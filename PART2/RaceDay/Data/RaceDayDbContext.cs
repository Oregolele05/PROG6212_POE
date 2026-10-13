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
    }
}