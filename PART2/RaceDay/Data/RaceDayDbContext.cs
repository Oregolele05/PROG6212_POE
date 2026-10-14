using Microsoft.EntityFrameworkCore;
using RaceDay.Models;

namespace RaceDay.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Organiser> Organisers { get; set; }
        public DbSet<EventGig> EventGigs { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<EventEnrollment> EventEnrollments { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Explicitly mark optional EventGig properties as NOT required ----
            modelBuilder.Entity<EventGig>()
                .Property(e => e.eventRoute)
                .IsRequired(false);

            modelBuilder.Entity<EventGig>()
                .Property(e => e.eventMap)
                .IsRequired(false);

            // ---- Cascade delete prevention ----
            modelBuilder.Entity<Result>()
                .HasOne(r => r.Participant)
                .WithMany()
                .HasForeignKey(r => r.participantID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.EventGig)
                .WithMany()
                .HasForeignKey(r => r.eventID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventEnrollment>()
                .HasOne(e => e.Participant)
                .WithMany()
                .HasForeignKey(e => e.participantID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventEnrollment>()
                .HasOne(e => e.EventGig)
                .WithMany()
                .HasForeignKey(e => e.eventID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}