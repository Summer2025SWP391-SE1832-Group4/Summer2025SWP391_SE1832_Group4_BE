using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.EntityTypeConfigurations
{
    /// <summary>
    /// System configuration type configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities.SystemConfiguration&gt;" />
    public class SystemConfigurationTypeConfiguration : IEntityTypeConfiguration<SystemConfiguration>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            // Primary Key
            builder.HasKey(e => e.Id);

            // Config the column.
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid");
            builder.Property(e => e.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
            builder.Property(e => e.Description).HasColumnName("description").HasColumnType("nvarchar").HasMaxLength(255);
            builder.Property(e => e.IsEnabled).HasColumnName("is_enable").HasDefaultValue(false);
            builder.Property(e => e.Value).HasColumnName("value").HasDefaultValue(false);

            // Default base entity property column.
            builder.Property(e => e.CreatedOn).HasColumnName("created_on").HasColumnType("timestaptz");
            builder.Property(e => e.ModifiedOn).HasColumnName("modified_on").HasColumnType("timestaptz");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        }
    }
}
