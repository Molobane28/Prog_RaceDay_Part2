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

        // 7 DbSets matching the 7 tables in the ERD
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

            // =========================================
            // USERS
            // =========================================
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_USERS_Role",
                    "[Role] IN ('Organiser', 'Participant')"));

            // =========================================
            // ORGANISERS  (1-to-1 with User)
            // =========================================
            modelBuilder.Entity<Organiser>()
                .HasIndex(o => o.UserID)
                .IsUnique();

            modelBuilder.Entity<Organiser>()
                .HasOne(o => o.User)
                .WithOne(u => u.Organiser)
                .HasForeignKey<Organiser>(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================
            // PARTICIPANTS  (1-to-1 with User)
            // =========================================
            modelBuilder.Entity<Participant>()
                .HasIndex(p => p.UserID)
                .IsUnique();

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.User)
                .WithOne(u => u.Participant)
                .HasForeignKey<Participant>(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================
            // EVENTS
            // =========================================
            modelBuilder.Entity<Event>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_EVENTS_MaxParticipants",
                    "[MaxParticipants] > 0"));

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================================
            // CATEGORIES
            // =========================================
            modelBuilder.Entity<Category>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_CATEGORIES_Distance",
                    "[Distance] > 0"));

            modelBuilder.Entity<Category>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_CATEGORIES_EntryFee",
                    "[EntryFee] >= 0"));

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Event)
                .WithMany(e => e.Categories)
                .HasForeignKey(c => c.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================
            // EVENT ENROLMENTS
            // =========================================
            modelBuilder.Entity<EventEnrolment>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_ENROLMENTS_PaymentAmount",
                    "[PaymentAmount] >= 0"));

            modelBuilder.Entity<EventEnrolment>()
                .HasOne(e => e.Participant)
                .WithMany(p => p.EventEnrolments)
                .HasForeignKey(e => e.ParticipantID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventEnrolment>()
                .HasOne(e => e.Category)
                .WithMany(c => c.EventEnrolments)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventEnrolment>()
                .HasOne(e => e.Event)
                .WithMany(ev => ev.EventEnrolments)
                .HasForeignKey(e => e.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================================
            // RESULTS  (1-to-1 with EventEnrolment)
            // =========================================
            modelBuilder.Entity<Result>()
                .HasIndex(r => r.EnrolmentID)
                .IsUnique();

            modelBuilder.Entity<Result>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_RESULTS_Position",
                    "[Position] IS NULL OR [Position] > 0"));

            modelBuilder.Entity<Result>()
                .HasOne(r => r.EventEnrolment)
                .WithOne(e => e.Result)
                .HasForeignKey<Result>(r => r.EnrolmentID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}