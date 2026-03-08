namespace SmartVideoCallApp.Data.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MeetingRoomId { get; set; }
        public Guid SenderUserId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;

        public MeetingRoom? MeetingRoom { get; set; }
        public UserAccount? SenderUser { get; set; }
    }
}
