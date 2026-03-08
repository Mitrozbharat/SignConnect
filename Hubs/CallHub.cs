using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SmartVideoCallApp.Data;
using SmartVideoCallApp.Data.Entities;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace SmartVideoCallApp.Hubs
{
    public class PeerInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class RoomInfo
    {
        public bool Exists { get; set; }
        public string Visibility { get; set; } = string.Empty;
    }

    public class CallHub : Hub
    {
        private readonly AppSignDbContext _db;

        // Tracks roomId -> (ConnectionId -> UserName)
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> RoomParticipants = new();
        // Tracks ConnectionId -> RoomId
        private static readonly ConcurrentDictionary<string, string> UserRooms = new();
        // Tracks ConnectionId -> UserName
        private static readonly ConcurrentDictionary<string, string> UserNames = new();
        // Tracks ConnectionId -> UserAccount.Id
        private static readonly ConcurrentDictionary<string, Guid> ConnectionUsers = new();
        // Tracks roomId -> Visibility ("public", "private", "org")
        private static readonly ConcurrentDictionary<string, string> RoomVisibility = new();
        // Tracks roomId -> Organization Name
        private static readonly ConcurrentDictionary<string, string> RoomOrg = new();
        // Tracks roomId -> Host ConnectionId
        private static readonly ConcurrentDictionary<string, string> RoomHosts = new();

        public CallHub(AppSignDbContext db)
        {
            _db = db;
        }

        public async Task RequestToJoin(string roomId, string userName, string userOrg, string meetingType)
        {
            try
            {
                Console.WriteLine($"[HUB] RequestToJoin from {userName} for room {roomId}");

                var user = await EnsureUserAsync(userName, userOrg);
                var room = await EnsureRoomAsync(roomId, meetingType, user.Id, userOrg);
                await AddActivityAsync(room.Id, user.Id, ActivityType.JoinRequested, $"Join request by {userName}");

                // If room doesn't exist yet OR host is gone, the first person can be the host
                bool hostIsConnected = false;
                if (RoomHosts.TryGetValue(roomId, out var existingHost))
                {
                    hostIsConnected = !string.IsNullOrEmpty(existingHost) && UserRooms.ContainsKey(existingHost);
                }

                if (!hostIsConnected)
                {
                    Console.WriteLine($"[HUB] Room {roomId} has no active host. Assigning {userName} as host.");
                    RoomHosts[roomId] = Context.ConnectionId;
                    await JoinRoom(roomId, userName, userOrg, meetingType);
                    return;
                }

                var visibility = RoomVisibility.GetValueOrDefault(roomId, "private");
                var requiredOrg = RoomOrg.GetValueOrDefault(roomId, "");

                if (visibility == "org" &&
                    !string.IsNullOrEmpty(requiredOrg) &&
                    string.Equals(userOrg, requiredOrg, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"[HUB] Auto-accepting {userName} for org room {roomId}");
                    await AddActivityAsync(room.Id, user.Id, ActivityType.JoinApproved, $"Join auto-approved for {userName}");
                    await Clients.Caller.SendAsync("JoinAccepted", roomId, visibility);
                }
                else
                {
                    // Public, Private or Org mismatch -> Must knock
                    if (RoomHosts.TryGetValue(roomId, out var hostId))
                    {
                        Console.WriteLine($"[HUB] Sending JoinRequest to host {hostId} for user {userName}");
                        await Clients.Client(hostId).SendAsync("JoinRequest", Context.ConnectionId, userName);
                    }
                    else
                    {
                        await JoinRoom(roomId, userName, userOrg, visibility);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HUB ERROR] RequestToJoin FAIL: {ex.Message}");
                throw;
            }
        }

        public async Task AcceptJoin(string targetConnectionId, string roomId)
        {
            var visibility = RoomVisibility.GetValueOrDefault(roomId, "private");
            Console.WriteLine($"[HUB] Host accepted {targetConnectionId} for room {roomId}");

            if (ConnectionUsers.TryGetValue(targetConnectionId, out var userId))
            {
                var room = await _db.MeetingRooms.AsNoTracking().FirstOrDefaultAsync(r => r.RoomCode == roomId);
                await AddActivityAsync(room?.Id, userId, ActivityType.JoinApproved, $"Host approved join for {targetConnectionId}");
            }

            await Clients.Client(targetConnectionId).SendAsync("JoinAccepted", roomId, visibility);
        }

        public async Task RejectJoin(string targetConnectionId)
        {
            Console.WriteLine($"[HUB] Host rejected {targetConnectionId}");

            if (ConnectionUsers.TryGetValue(targetConnectionId, out var userId))
            {
                await AddActivityAsync(null, userId, ActivityType.JoinRejected, $"Host rejected join for {targetConnectionId}");
            }

            await Clients.Client(targetConnectionId).SendAsync("JoinRejected");
        }

        public async Task<RoomInfo> GetRoomInfo(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                return new RoomInfo { Exists = false, Visibility = string.Empty };
            }

            var memoryExists = RoomHosts.ContainsKey(roomId) || RoomParticipants.ContainsKey(roomId) || RoomVisibility.ContainsKey(roomId);
            var memoryVisibility = RoomVisibility.GetValueOrDefault(roomId, string.Empty);
            if (memoryExists && !string.IsNullOrWhiteSpace(memoryVisibility))
            {
                return new RoomInfo { Exists = true, Visibility = memoryVisibility };
            }

            var room = await _db.MeetingRooms.AsNoTracking().FirstOrDefaultAsync(r => r.RoomCode == roomId);
            if (room == null)
            {
                return new RoomInfo { Exists = false, Visibility = string.Empty };
            }

            return new RoomInfo
            {
                Exists = true,
                Visibility = ToVisibilityString(room.Visibility)
            };
        }

        public async Task JoinRoom(string roomId, string userName, string userOrg = "", string visibility = "public")
        {
            try
            {
                Console.WriteLine($"[HUB ENTER] JoinRoom: roomId={roomId}, userName={userName}, userOrg={userOrg}, visibility={visibility}");

                if (string.IsNullOrEmpty(roomId))
                {
                    throw new HubException("RoomId is required.");
                }
                if (string.IsNullOrEmpty(userName))
                {
                    throw new HubException("UserName is required.");
                }

                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                UserRooms[Context.ConnectionId] = roomId;
                UserNames[Context.ConnectionId] = userName;

                var user = await EnsureUserAsync(userName, userOrg);
                ConnectionUsers[Context.ConnectionId] = user.Id;

                // If this is the first person, they are the host
                if (RoomHosts.TryAdd(roomId, Context.ConnectionId))
                {
                    RoomVisibility[roomId] = visibility;
                    RoomOrg[roomId] = userOrg;
                }
                else
                {
                    RoomVisibility.TryAdd(roomId, visibility);
                    RoomOrg.TryAdd(roomId, userOrg);
                }

                var users = RoomParticipants.GetOrAdd(roomId, _ => new ConcurrentDictionary<string, string>());
                users[Context.ConnectionId] = userName;

                var currentVisibility = RoomVisibility.GetValueOrDefault(roomId, visibility);
                bool isActuallyHost = RoomHosts.GetValueOrDefault(roomId) == Context.ConnectionId;

                var room = await EnsureRoomAsync(roomId, currentVisibility, user.Id, userOrg);
                if (isActuallyHost && room.HostUserId != user.Id)
                {
                    room.HostUserId = user.Id;
                    await _db.SaveChangesAsync();
                }

                await AddActivityAsync(room.Id, user.Id, ActivityType.UserJoined, $"{userName} joined room {roomId}");

                var others = users
                    .Where(kvp => kvp.Key != Context.ConnectionId)
                    .Select(kvp => new PeerInfo { Id = kvp.Key, Name = kvp.Value })
                    .ToList();

                await Clients.OthersInGroup(roomId).SendAsync("UserJoined", userName, Context.ConnectionId);
                await Clients.Caller.SendAsync("JoinedRoom", Context.ConnectionId, others, currentVisibility, isActuallyHost);
                await BroadcastUserCount(roomId);

                Console.WriteLine($"[HUB EXIT] JoinRoom SUCCESS for {userName} ({Context.ConnectionId})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HUB ERROR] JoinRoom FAIL for {userName}: {ex}");
                if (ex is HubException) throw;
                throw new HubException("Failed to join room: " + ex.Message);
            }
        }

        public async Task SendMessage(string roomId, string user, string message)
        {
            try
            {
                await Clients.Group(roomId).SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("HH:mm"));

                if (string.IsNullOrWhiteSpace(message))
                {
                    return;
                }

                var userId = ConnectionUsers.GetValueOrDefault(Context.ConnectionId);
                if (userId == Guid.Empty)
                {
                    userId = (await EnsureUserAsync(user, "")).Id;
                }

                var room = await _db.MeetingRooms.FirstOrDefaultAsync(r => r.RoomCode == roomId);
                if (room == null)
                {
                    room = await EnsureRoomAsync(roomId, "private", userId, "");
                }

                _db.ChatMessages.Add(new ChatMessage
                {
                    MeetingRoomId = room.Id,
                    SenderUserId = userId,
                    MessageText = message
                });
                await _db.SaveChangesAsync();

                await AddActivityAsync(room.Id, userId, ActivityType.MessageSent, $"{user} sent a message");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HUB ERROR] SendMessage FAIL: {ex.Message}");
            }
        }

        // public async Task SaveSignCoordinates(string roomId, string targetId, object coordinates)
        // {
        //     try
        //     {
        //         if (string.IsNullOrWhiteSpace(roomId))
        //         {
        //             return;
        //         }

        //         var room = await _db.MeetingRooms.AsNoTracking().FirstOrDefaultAsync(r => r.RoomCode == roomId);
        //         var hasUser = ConnectionUsers.TryGetValue(Context.ConnectionId, out var userId);

        //         var payload = JsonSerializer.Serialize(new
        //         {
        //             roomId,
        //             targetId,
        //             connectionId = Context.ConnectionId,
        //             capturedAtUtc = DateTime.UtcNow,
        //             coordinates
        //         });

        //         await AddActivityAsync(
        //             room?.Id,
        //             hasUser ? userId : null,
        //             ActivityType.SignCoordinatesCaptured,
        //             $"Sign coordinates captured for {targetId}",
        //             payload);
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"[HUB ERROR] SaveSignCoordinates FAIL: {ex.Message}");
        //     }
        // }

        public async Task UpdateMediaStatus(string roomId, bool isCameraOn, bool isMicOn)
        {
            try
            {
                await Clients.OthersInGroup(roomId).SendAsync("MediaStatusChanged", Context.ConnectionId, isCameraOn, isMicOn);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HUB ERROR] UpdateMediaStatus FAIL: {ex.Message}");
            }
        }

        public async Task KickUser(string roomId, string targetConnectionId)
        {
            try
            {
                if (RoomHosts.TryGetValue(roomId, out var hostConnectionId) && hostConnectionId == Context.ConnectionId)
                {
                    await Clients.Client(targetConnectionId).SendAsync("Kicked");
                }
                else
                {
                    Console.WriteLine($"[HUB ERROR] Unauthorized kick attempt by {Context.ConnectionId} for room {roomId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HUB ERROR] KickUser FAIL: {ex.Message}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (UserRooms.TryRemove(Context.ConnectionId, out var roomId))
            {
                UserNames.TryRemove(Context.ConnectionId, out _);
                ConnectionUsers.TryRemove(Context.ConnectionId, out var userId);

                if (RoomParticipants.TryGetValue(roomId, out var users))
                {
                    users.TryRemove(Context.ConnectionId, out _);
                    if (users.IsEmpty)
                    {
                        RoomParticipants.TryRemove(roomId, out _);
                        RoomHosts.TryRemove(roomId, out _);

                        var room = await _db.MeetingRooms.FirstOrDefaultAsync(r => r.RoomCode == roomId);
                        if (room != null && room.Status != RoomStatus.Ended)
                        {
                            room.Status = RoomStatus.Ended;
                            room.EndedAtUtc = DateTime.UtcNow;
                            await _db.SaveChangesAsync();
                        }
                    }
                    else if (RoomHosts.TryGetValue(roomId, out var hostId) && hostId == Context.ConnectionId)
                    {
                        var nextHost = users.Keys.FirstOrDefault();
                        if (nextHost != null) RoomHosts[roomId] = nextHost;
                    }
                }

                await AddActivityAsync(
                    (await _db.MeetingRooms.AsNoTracking().FirstOrDefaultAsync(r => r.RoomCode == roomId))?.Id,
                    userId == Guid.Empty ? null : userId,
                    ActivityType.UserLeft,
                    $"Connection {Context.ConnectionId} left room {roomId}");

                await Clients.Group(roomId).SendAsync("UserLeft", Context.ConnectionId);
                await BroadcastUserCount(roomId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        private async Task BroadcastUserCount(string roomId)
        {
            if (RoomParticipants.TryGetValue(roomId, out var users))
            {
                await Clients.Group(roomId).SendAsync("UpdateUserCount", users.Count);
            }
        }

        // WebRTC Signaling (Targeted)
        public async Task SendOffer(string targetId, object offer)
        {
            await Clients.Client(targetId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
        }

        public async Task SendAnswer(string targetId, object answer)
        {
            await Clients.Client(targetId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
        }

        public async Task SendIceCandidate(string targetId, object candidate)
        {
            await Clients.Client(targetId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
        }

        private async Task<Organization?> EnsureOrganizationAsync(string orgName)
        {
            var normalized = (orgName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return null;
            }

            var org = await _db.Organizations.FirstOrDefaultAsync(o => o.Name.ToLower() == normalized.ToLower());
            if (org != null)
            {
                return org;
            }

            org = new Organization
            {
                Name = normalized,
                Code = $"ORG-{Guid.NewGuid():N}".Substring(0, 12).ToUpperInvariant()
            };
            _db.Organizations.Add(org);
            await _db.SaveChangesAsync();
            return org;
        }

        private async Task<UserAccount> EnsureUserAsync(string userName, string userOrg)
        {
            var displayName = string.IsNullOrWhiteSpace(userName) ? "Guest" : userName.Trim();
            var org = await EnsureOrganizationAsync(userOrg);
            var orgId = org?.Id;

            var user = await _db.UserAccounts
                .FirstOrDefaultAsync(u => u.DisplayName == displayName && u.OrganizationId == orgId);

            if (user != null)
            {
                user.LastSeenAtUtc = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return user;
            }

            var slug = Regex.Replace(displayName.ToLowerInvariant(), "[^a-z0-9]+", ".");
            slug = slug.Trim('.');
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = "user";
            }

            user = new UserAccount
            {
                DisplayName = displayName,
                Email = $"{slug}.{Guid.NewGuid():N}@appsign.local",
                OrganizationId = org?.Id,
                LastSeenAtUtc = DateTime.UtcNow
            };
            _db.UserAccounts.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        private async Task<MeetingRoom> EnsureRoomAsync(string roomCode, string visibility, Guid hostUserId, string userOrg)
        {
            var room = await _db.MeetingRooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);
            var vis = ParseVisibility(visibility);
            var org = await EnsureOrganizationAsync(userOrg);

            if (room != null)
            {
                if (room.Visibility != vis)
                {
                    room.Visibility = vis;
                }
                if (vis == MeetingVisibility.Organization && org != null)
                {
                    room.OrganizationId = org.Id;
                }
                if (room.Status == RoomStatus.Ended)
                {
                    room.Status = RoomStatus.Active;
                    room.EndedAtUtc = null;
                }

                await _db.SaveChangesAsync();
                return room;
            }

            room = new MeetingRoom
            {
                RoomCode = roomCode,
                Visibility = vis,
                Status = RoomStatus.Active,
                HostUserId = hostUserId,
                OrganizationId = vis == MeetingVisibility.Organization ? org?.Id : null
            };
            _db.MeetingRooms.Add(room);
            await _db.SaveChangesAsync();

            await AddActivityAsync(room.Id, hostUserId, ActivityType.RoomCreated, $"Room {roomCode} created");
            return room;
        }

        private async Task AddActivityAsync(Guid? roomId, Guid? userId, ActivityType type, string description, string? metadataJson = null)
        {
            _db.ActivityLogs.Add(new ActivityLog
            {
                MeetingRoomId = roomId,
                UserAccountId = userId,
                ActivityType = type,
                Description = description
                ,
                MetadataJson = metadataJson
            });
            await _db.SaveChangesAsync();
        }

        private static MeetingVisibility ParseVisibility(string visibility)
        {
            return (visibility ?? string.Empty).Trim().ToLowerInvariant() switch
            {
                "org" => MeetingVisibility.Organization,
                "organization" => MeetingVisibility.Organization,
                "private" => MeetingVisibility.Private,
                _ => MeetingVisibility.Public
            };
        }

        private static string ToVisibilityString(MeetingVisibility visibility)
        {
            return visibility switch
            {
                MeetingVisibility.Organization => "org",
                MeetingVisibility.Private => "private",
                _ => "public"
            };
        }
    }
}
