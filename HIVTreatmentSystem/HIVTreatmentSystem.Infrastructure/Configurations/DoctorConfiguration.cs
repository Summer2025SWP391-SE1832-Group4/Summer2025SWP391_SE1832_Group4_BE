using HIVTreatmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HIVTreatmentSystem.Infrastructure.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            builder.HasKey(e => e.DoctorId);

            builder.Property(e => e.DoctorId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Specialty)
                .HasMaxLength(100);

            builder.Property(e => e.Qualifications)
                .HasMaxLength(500);

            builder.Property(e => e.ShortDescription)
                .HasMaxLength(1000);

            // Configure one-to-one relationship with Account
            builder.HasOne(e => e.Account)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship with Certificate
            builder.HasMany(e => e.Certificates)
                .WithOne(c => c.Doctor)
                .HasForeignKey(c => c.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 