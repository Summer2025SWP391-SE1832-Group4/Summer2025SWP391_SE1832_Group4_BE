using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HIVTreatmentSystem.Infrastructure.Data
{
    public class HIVDbContext : DbContext
    {
        public HIVDbContext(DbContextOptions<HIVDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; } = default!;
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<StandardARVRegimen> StandardARVRegimens { get; set; }
        public DbSet<PatientTreatment> PatientTreatments { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<EducationalMaterial> EducationalMaterials { get; set; }
        public DbSet<SystemAuditLog> SystemAuditLogs { get; set; }
        public DbSet<ExperienceWorking> ExperienceWorkings { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum conversions to string
            modelBuilder.Entity<Account>().Property(e => e.AccountStatus).HasConversion<string>();

            modelBuilder.Entity<Patient>().Property(e => e.Gender).HasConversion<string>();

            modelBuilder.Entity<Appointment>().Property(e => e.Status).HasConversion<string>();

            modelBuilder.Entity<PatientTreatment>().Property(e => e.Status).HasConversion<string>();

            modelBuilder.Entity<Reminder>().Property(e => e.ReminderType).HasConversion<string>();

            modelBuilder.Entity<Appointment>().Property(e => e.AppointmentType).HasConversion<string>();

            modelBuilder.Entity<Appointment>().Property(e => e.AppointmentService).HasConversion<string>();

            modelBuilder.Entity<Appointment>().Property(e => e.Status).HasConversion<string>();




            modelBuilder
                .Entity<ExperienceWorking>()
                .HasOne(e => e.Doctor)
                .WithMany(d => d.ExperienceWorkings)
                .HasForeignKey(e => e.DoctorId);

            modelBuilder
                .Entity<Blog>()
                .HasOne(b => b.BlogTag)
                .WithMany(t => t.Blogs)
                .HasForeignKey(b => b.BlogTagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Reminder>()
                .Property(e => e.NotificationMethod)
                .HasConversion<string>();

            modelBuilder
                .Entity<EducationalMaterial>()
                .Property(e => e.MaterialType)
                .HasConversion<string>();
            

            // Configure primary keys and relationships
            ConfigureRoleEntity(modelBuilder);
            ConfigureUserEntity(modelBuilder);
            ConfigurePatientEntity(modelBuilder);
            ConfigureDoctorEntity(modelBuilder);
            ConfigureStaffEntity(modelBuilder);
            ConfigureAppointmentEntity(modelBuilder);
            ConfigureMedicalRecordEntity(modelBuilder);
            ConfigureTestResultEntity(modelBuilder);
            ConfigureStandardARVRegimenEntity(modelBuilder);
            ConfigurePatientTreatmentEntity(modelBuilder);
            ConfigureReminderEntity(modelBuilder);
            ConfigureEducationalMaterialEntity(modelBuilder);
            ConfigureSystemAuditLogEntity(modelBuilder);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void ConfigureRoleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.HasIndex(e => e.RoleName).IsUnique();
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
            });
        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ProfileImageUrl).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity
                    .HasOne(e => e.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigurePatientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId);
                entity.HasIndex(e => e.PatientCodeAtFacility).IsUnique();
                entity.HasIndex(e => e.AnonymousIdentifier).IsUnique();

                entity.Property(e => e.PatientCodeAtFacility).HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.AnonymousIdentifier).HasMaxLength(50);

                entity
                    .HasOne(e => e.Account)
                    .WithOne(u => u.Patient)
                    .HasForeignKey<Patient>(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(e => e.MedicalRecord)
                    .WithOne(m => m.Patient)
                    .HasForeignKey<MedicalRecord>(m => m.PatientId) // FK nằm ở MedicalRecord
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureDoctorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DoctorId);
                entity.Property(e => e.Specialty).HasMaxLength(100);
                entity.Property(e => e.ShortDescription).HasMaxLength(500);

                entity
                    .HasOne(e => e.Account)
                    .WithOne(u => u.Doctor)
                    .HasForeignKey<Doctor>(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureStaffEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.StaffId);
                entity.Property(e => e.Position).HasMaxLength(100);

                entity
                    .HasOne(e => e.Account)
                    .WithOne(u => u.Staff)
                    .HasForeignKey<Staff>(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private void ConfigureAppointmentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.AppointmentId);
                entity.Property(e => e.AppointmentType).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.AppointmentType)
                        .HasConversion<string>();

                entity.Property(e => e.AppointmentService)
                        .HasConversion<string>()
                        .IsRequired(false);
                entity
                    .HasOne(e => e.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.Doctor)
                    .WithMany(d => d.Appointments)
                    .HasForeignKey(e => e.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedAppointments)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.SetNull);

            });
        }

        private void ConfigureMedicalRecordEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(e => e.MedicalRecordId);
                entity.Property(e => e.CoinfectionDiseases).HasMaxLength(255);

                // 1-to-1 relationship with Patient
                entity
                    .HasOne(e => e.Patient)
                    .WithOne(p => p.MedicalRecord)
                    .HasForeignKey<MedicalRecord>(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Many-to-1 relationship with Doctor
                entity
                    .HasOne(e => e.Doctor)
                    .WithMany(d => d.MedicalRecords)
                    .HasForeignKey(e => e.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index to ensure unique Patient-MedicalRecord relationship
                entity.HasIndex(e => e.PatientId).IsUnique();
            });
        }

        private void ConfigureTestResultEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestResult>(entity =>
            {
                entity.HasKey(e => e.TestResultId);
                entity.Property(e => e.TestType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.HivViralLoadValue).HasMaxLength(50);
                entity.Property(e => e.LabName).HasMaxLength(100);
                entity.Property(e => e.TestResults).HasMaxLength(255);

                entity
                    .HasOne(e => e.Patient)
                    .WithMany(p => p.TestResults)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.MedicalRecord)
                    .WithMany(m => m.TestResults)
                    .HasForeignKey(e => e.MedicalRecordId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity
                    .HasOne(e => e.Appointment)
                    .WithMany(a => a.TestResults)
                    .HasForeignKey(e => e.AppointmentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void ConfigureStandardARVRegimenEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StandardARVRegimen>(entity =>
            {
                entity.HasKey(e => e.RegimenId);
                entity.HasIndex(e => e.RegimenName).IsUnique();
                entity.Property(e => e.RegimenName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.TargetPopulation).HasMaxLength(100);
                entity.Property(e => e.StandardDosage).HasMaxLength(255);
            });
        }

        private void ConfigurePatientTreatmentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientTreatment>(entity =>
            {
                entity.HasKey(e => e.PatientTreatmentId);
                entity.Property(e => e.ActualDosage).HasMaxLength(255);

                entity
                    .HasOne(e => e.Patient)
                    .WithMany(p => p.Treatments)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.Regimen)
                    .WithMany(r => r.PatientTreatments)
                    .HasForeignKey(e => e.RegimenId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.PrescribingDoctor)
                    .WithMany(d => d.PrescribedTreatments)
                    .HasForeignKey(e => e.PrescribingDoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureReminderEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.HasKey(e => e.ReminderId);
                entity.Property(e => e.Frequency).HasMaxLength(50);
                entity.Property(e => e.ReminderContent).HasMaxLength(500);

                entity
                    .HasOne(e => e.Patient)
                    .WithMany(p => p.Reminders)
                    .HasForeignKey(e => e.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.PatientTreatment)
                    .WithMany(t => t.Reminders)
                    .HasForeignKey(e => e.PatientTreatmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity
                    .HasOne(e => e.Appointment)
                    .WithMany(a => a.Reminders)
                    .HasForeignKey(e => e.AppointmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Check constraint for reminder type
                entity.HasCheckConstraint(
                    "CK_Reminder_TreatmentOrAppointment",
                    "(ReminderType = 'TakeMedication' AND PatientTreatmentId IS NOT NULL AND AppointmentId IS NULL) OR "
                        + "(ReminderType = 'FollowUpAppointment' AND AppointmentId IS NOT NULL AND PatientTreatmentId IS NULL)"
                );
            });
        }

        private void ConfigureEducationalMaterialEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EducationalMaterial>(entity =>
            {
                entity.HasKey(e => e.MaterialId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.TargetAudience).HasMaxLength(100);
                entity.Property(e => e.PublishedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.ViewCount).HasDefaultValue(0);
                entity.Property(e => e.IsPublished).HasDefaultValue(true);

                entity
                    .HasOne(e => e.Author)
                    .WithMany(u => u.AuthoredMaterials)
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
        

        private void ConfigureSystemAuditLogEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemAuditLog>(entity =>
            {
                entity.HasKey(e => e.LogId);
                entity.Property(e => e.Timestamp).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.RoleAtTimeOfAction).HasMaxLength(50);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(255);
                entity.Property(e => e.AffectedEntity).HasMaxLength(100);
                entity.Property(e => e.AffectedEntityId).HasMaxLength(100);
                entity.Property(e => e.IpAddress).HasMaxLength(45);

                entity
                    .HasOne(e => e.Account)
                    .WithMany(u => u.AuditLogs)
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder
                .Entity<Role>()
                .HasData(
                    new Role
                    {
                        RoleId = 1,
                        RoleName = "Admin",
                        Description = "System Administrator",
                    },
                    new Role
                    {
                        RoleId = 2,
                        RoleName = "Manager",
                        Description = "Healthcare Facility Manager",
                    },
                    new Role
                    {
                        RoleId = 3,
                        RoleName = "Doctor",
                        Description = "Medical Doctor",
                    },
                    new Role
                    {
                        RoleId = 4,
                        RoleName = "Staff",
                        Description = "Healthcare Staff (Receptionist, Nurse, etc.)",
                    },
                    new Role
                    {
                        RoleId = 5,
                        RoleName = "Patient",
                        Description = "Patient/Client",
                    }
                );

            // Seed Sample Standard ARV Regimens
            modelBuilder
                .Entity<StandardARVRegimen>()
                .HasData(
                    new StandardARVRegimen
                    {
                        RegimenId = 1,
                        RegimenName = "TDF + 3TC + EFV",
                        DetailedDescription =
                            "Tenofovir Disoproxil Fumarate + Lamivudine + Efavirenz",
                        TargetPopulation = "First-line treatment for adults",
                        StandardDosage = "One tablet once daily",
                        Contraindications = "Severe renal impairment, psychiatric disorders",
                        CommonSideEffects = "Dizziness, abnormal dreams, rash, nausea",
                    },
                    new StandardARVRegimen
                    {
                        RegimenId = 2,
                        RegimenName = "ABC + 3TC + DTG",
                        DetailedDescription = "Abacavir + Lamivudine + Dolutegravir",
                        TargetPopulation = "Alternative first-line treatment",
                        StandardDosage = "One tablet once daily",
                        Contraindications = "HLA-B*5701 positive patients",
                        CommonSideEffects = "Headache, insomnia, fatigue",
                    },
                    new StandardARVRegimen
                    {
                        RegimenId = 3,
                        RegimenName = "AZT + 3TC + LPV/r",
                        DetailedDescription = "Zidovudine + Lamivudine + Lopinavir/ritonavir",
                        TargetPopulation = "Second-line treatment",
                        StandardDosage = "Twice daily dosing",
                        Contraindications = "Severe anemia, neutropenia",
                        CommonSideEffects = "Nausea, diarrhea, lipodystrophy",
                    }
                );
        }
    }
}
