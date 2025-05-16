using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.EntityTypeConfigurations
{
    /// <summary>
    /// Old password configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities.OldPassword&gt;" />
    public class OldPasswordTypeConfiguration : IEntityTypeConfiguration<OldPassword>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<OldPassword> builder)
        {
            // Primary Key
            builder.HasKey(e => e.Id);

            // Config the column.
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid");
            builder.Property(e => e.OldPasswordHash).HasColumnName("old_password_hash").HasColumnType("nvarchar").HasMaxLength(255);

            // Default base entity property column.
            builder.Property(e => e.CreatedOn).HasColumnName("created_on").HasColumnType("timestaptz");
            builder.Property(e => e.ModifiedOn).HasColumnName("modified_on").HasColumnType("timestaptz");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            // Foregin Key
            builder.HasOne(e => e.Account)
            .WithMany(a => a.OldPasswords)
            .HasForeignKey(e => e.AccountId)
            .IsRequired(false);
        }
    }
}
