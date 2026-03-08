using Microsoft.EntityFrameworkCore;
using SmartVideoCallApp.Data.Entities;

namespace SmartVideoCallApp.Data
{
    public class AppSignDbContext : DbContext
    {
        public AppSignDbContext(DbContextOptions<AppSignDbContext> options) : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<MeetingRoom> MeetingRooms => Set<MeetingRoom>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
        public DbSet<SignCoordinate> SignCoordinates => Set<SignCoordinate>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.DisplayName).HasMaxLength(120).IsRequired();
                entity.Property(x => x.Email).HasMaxLength(180).IsRequired();
                entity.HasIndex(x => x.Email).IsUnique();
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
                entity.Property(x => x.Code).HasMaxLength(30).IsRequired();
                entity.HasIndex(x => x.Code).IsUnique();
            });

            modelBuilder.Entity<MeetingRoom>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.RoomCode).HasMaxLength(32).IsRequired();
                entity.HasIndex(x => x.RoomCode).IsUnique();
            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.MessageText).HasMaxLength(2000).IsRequired();
                entity.HasIndex(x => new { x.MeetingRoomId, x.SentAtUtc });
            });

            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Description).HasMaxLength(500).IsRequired();
                entity.HasIndex(x => x.CreatedAtUtc);
            });

            modelBuilder.Entity<SignCoordinate>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Label).HasMaxLength(120).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(300);
                entity.Property(x => x.CoordinatesJson).HasColumnType("longtext").IsRequired();
                entity.HasIndex(x => x.Label);
                entity.HasIndex(x => x.TimeId);
                entity.HasIndex(x => x.CreatedAtUtc);
            });
        }
    }
}
