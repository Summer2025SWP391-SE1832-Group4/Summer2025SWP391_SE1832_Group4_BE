using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HIVTreatmentSystem.Infrastructure.Data
{
    public class ARVManagementDbContext : DbContext
    {
        public ARVManagementDbContext(DbContextOptions<ARVManagementDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<ARVStandard> ARVStandards { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<ExperienceWorking> ExperienceWorkings { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<CD4Test> CD4Tests { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSlot> BookingSlots { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SystemMetric> SystemMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum conversions
            modelBuilder.Entity<User>().Property(e => e.Role).HasConversion<int>();

            modelBuilder.Entity<PatientRecord>().Property(e => e.Status).HasConversion<int>();

            modelBuilder.Entity<Booking>().Property(e => e.Status).HasConversion<int>();

            modelBuilder.Entity<CD4Test>().Property(e => e.Result).HasConversion<int>();

            modelBuilder.Entity<Blog>().Property(e => e.Status).HasConversion<int>();

            modelBuilder
                .Entity<Certificate>()
                .Property(e => e.CertificateType)
                .HasConversion<int>();

            modelBuilder.Entity<Reminder>().Property(e => e.Type).HasConversion<int>();

            // Configure decimal precision
            modelBuilder.Entity<CD4Test>().Property(e => e.CD4Count).HasPrecision(10, 2);

            modelBuilder.Entity<CD4Test>().Property(e => e.ViralLoad).HasPrecision(15, 2);

            modelBuilder.Entity<SystemMetric>().Property(e => e.MetricValue).HasPrecision(18, 2);

            // Configure unique constraints
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<PatientRecord>().HasIndex(p => p.PatientCode).IsUnique();

            // Configure relationships
            modelBuilder
                .Entity<PatientRecord>()
                .HasOne(p => p.Doctor)
                .WithMany(u => u.PatientRecords)
                .HasForeignKey(p => p.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<PatientRecord>()
                .HasOne(p => p.ARVStandard)
                .WithMany(a => a.PatientRecords)
                .HasForeignKey(p => p.ARVStandardId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<ExperienceWorking>()
                .HasOne(e => e.Patient)
                .WithMany(p => p.ExperienceWorkings)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<ExperienceWorking>()
                .HasOne(e => e.Doctor)
                .WithMany(u => u.ExperienceWorkings)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Certificate>()
                .HasOne(c => c.Patient)
                .WithMany(p => p.Certificates)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Reminder>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reminders)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Reminder>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.PatientReminders)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<CD4Test>()
                .HasOne(c => c.Patient)
                .WithMany(p => p.CD4Tests)
                .HasForeignKey(c => c.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Booking>()
                .HasOne(b => b.Patient)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Booking>()
                .HasOne(b => b.Doctor)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Booking>()
                .HasOne(b => b.Slot)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.SlotId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<BookingSlot>()
                .HasOne(s => s.Doctor)
                .WithMany(u => u.BookingSlots)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Feedback>()
                .HasOne(f => f.Booking)
                .WithMany(b => b.Feedbacks)
                .HasForeignKey(f => f.BookingId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Blog>()
                .HasOne(b => b.Author)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes for performance
            modelBuilder.Entity<PatientRecord>().HasIndex(p => p.Status);

            modelBuilder.Entity<Booking>().HasIndex(b => new { b.BookingDate, b.Status });

            modelBuilder.Entity<CD4Test>().HasIndex(c => c.TestDate);

            modelBuilder.Entity<Blog>().HasIndex(b => new { b.Status, b.PublishedDate });

            modelBuilder.Entity<Reminder>().HasIndex(r => new { r.ReminderDate, r.IsSent });

            // Configure soft delete filter
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PatientRecord>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<ARVStandard>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<ExperienceWorking>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Certificate>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Reminder>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<CD4Test>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<BookingSlot>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Feedback>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Blog>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<SystemMetric>().HasQueryFilter(e => !e.IsDeleted);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed default ARV Standards
            modelBuilder
                .Entity<ARVStandard>()
                .HasData(
                    new ARVStandard
                    {
                        Id = 1,
                        Name = "WHO Standard 2023",
                        Description = "World Health Organization ARV Treatment Standard 2023",
                        Version = "1.0",
                        EffectiveDate = new DateTime(2023, 1, 1),
                        IsActive = true,
                        Guidelines = "Standard WHO guidelines for ARV treatment",
                        CreatedAt = DateTime.UtcNow,
                    },
                    new ARVStandard
                    {
                        Id = 2,
                        Name = "Vietnam MOH Standard 2023",
                        Description = "Vietnam Ministry of Health ARV Treatment Standard 2023",
                        Version = "2.0",
                        EffectiveDate = new DateTime(2023, 3, 1),
                        IsActive = true,
                        Guidelines = "Vietnam specific ARV treatment guidelines",
                        CreatedAt = DateTime.UtcNow,
                    }
                );

            // Seed admin user
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Email = "admin@example.com",
                        PasswordHash = "hashedpassword",
                        EmailVerificationToken = Guid.NewGuid().ToString(),
                        // ... other properties
                    }
                );
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
