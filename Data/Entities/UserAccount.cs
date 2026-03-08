namespace SmartVideoCallApp.Data.Entities
{
    public class UserAccount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid? OrganizationId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? LastSeenAtUtc { get; set; }

        public Organization? Organization { get; set; }
        public ICollection<MeetingRoom> HostedRooms { get; set; } = new List<MeetingRoom>();
        public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}
