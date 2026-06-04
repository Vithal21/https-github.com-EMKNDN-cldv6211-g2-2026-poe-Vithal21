using Microsoft.EntityFrameworkCore;
using Event_Ease.Models;

namespace Event_Ease.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany()
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EventType>().HasData(
                new EventType
                {
                    EventTypeId = 1,
                    TypeName = "Wedding"
                },
                new EventType
                {
                    EventTypeId = 2,
                    TypeName = "Conference"
                },
                new EventType
                {
                    EventTypeId = 3,
                    TypeName = "Birthday"
                },
                new EventType
                {
                    EventTypeId = 4,
                    TypeName = "Corporate"
                },
                new EventType
                {
                    EventTypeId = 5,
                    TypeName = "Concert"
                }
            );
        }
    }
}