namespace SmartVideoCallApp.Data.Entities
{
    public class ActivityLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? MeetingRoomId { get; set; }
        public Guid? UserAccountId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? MetadataJson { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public MeetingRoom? MeetingRoom { get; set; }
        public UserAccount? UserAccount { get; set; }
    }
}
