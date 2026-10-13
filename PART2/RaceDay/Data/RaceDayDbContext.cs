using Microsoft.EntityFrameworkCore;
using RaceDay.Models;
using System.Collections.Generic;

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
        public DbSet<Result> Result { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent multiple cascade delete paths for RaceResult
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

            // Prevent multiple cascade delete paths for EventEnrollment
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