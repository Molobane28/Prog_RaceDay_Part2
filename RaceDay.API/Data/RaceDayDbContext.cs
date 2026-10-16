using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Organiser> Organisers { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<EventEnrolment> EventEnrolments { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Table names mapping to match Part 1 ERD exactly
            modelBuilder.Entity<User>().ToTable("USERS");
            modelBuilder.Entity<Organiser>().ToTable("ORGANISERS");
            modelBuilder.Entity<Participant>().ToTable("PARTICIPANTS");
            modelBuilder.Entity<Event>().ToTable("EVENTS");
            modelBuilder.Entity<Category>().ToTable("CATEGORIES");
            modelBuilder.Entity<EventEnrolment>().ToTable("EVENT_ENROLMENTS");
            modelBuilder.Entity<Result>().ToTable("RESULTS");

            // Relationship Configurations (Fix for line 119)
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Enrolments)
                .WithOne(ee => ee.Event)
                .HasForeignKey(ee => ee.EventID);

            modelBuilder.Entity<EventEnrolment>()
                .HasOne(ee => ee.Participant)
                .WithMany(p => p.Enrolments)
                .HasForeignKey(ee => ee.ParticipantID);

            modelBuilder.Entity<EventEnrolment>()
                .HasOne(ee => ee.Category)
                .WithMany(c => c.Enrolments)
                .HasForeignKey(ee => ee.CategoryID);

            modelBuilder.Entity<Result>()
                .HasOne(r => r.EventEnrolment)
                .WithOne(ee => ee.Result)
                .HasForeignKey<Result>(r => r.EnrolmentID);
        }
    }
}