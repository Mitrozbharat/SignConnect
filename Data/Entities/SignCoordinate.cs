namespace SmartVideoCallApp.Data.Entities
{
    public class SignCoordinate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Label { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long TimeId { get; set; }
        public string CoordinatesJson { get; set; } = "[]";
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
