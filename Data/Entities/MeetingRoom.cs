namespace SmartVideoCallApp.Data.Entities
{
    public class MeetingRoom
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RoomCode { get; set; } = string.Empty;
        public string? Title { get; set; }
        public MeetingVisibility Visibility { get; set; } = MeetingVisibility.Public;
        public RoomStatus Status { get; set; } = RoomStatus.Active;
        public Guid HostUserId { get; set; }
        public Guid? OrganizationId { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAtUtc { get; set; }

        public UserAccount? HostUser { get; set; }
        public Organization? Organization { get; set; }
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public ICollection<ActivityLog> Activities { get; set; } = new List<ActivityLog>();
    }
}
