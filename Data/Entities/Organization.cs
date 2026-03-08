namespace SmartVideoCallApp.Data.Entities
{
    public class Organization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<UserAccount> Users { get; set; } = new List<UserAccount>();
        public ICollection<MeetingRoom> Rooms { get; set; } = new List<MeetingRoom>();
    }
}
