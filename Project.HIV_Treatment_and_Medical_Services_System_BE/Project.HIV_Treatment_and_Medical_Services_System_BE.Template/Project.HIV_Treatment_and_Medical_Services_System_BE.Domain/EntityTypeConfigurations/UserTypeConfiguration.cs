using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.EntityTypeConfigurations
{
    /// <summary>
    /// User entity configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities.User&gt;" />
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities.User&gt;" />
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(user => user.Id);

            // Config the column.
            builder.Property(user => user.Id).HasColumnName("id").HasColumnType("uuid");
            builder.Property(user => user.Name).HasColumnName("name").HasColumnType("nvarchar").HasMaxLength(255);
            builder.Property(user => user.PhoneNumber).HasColumnName("phone_number").HasColumnType("nvarchar").HasMaxLength(255);
            builder.Property(user => user.Email).HasColumnName("email").HasColumnType("nvarchar").HasMaxLength(255);

            // Default base entity property column.
            builder.Property(u => u.CreatedOn).HasColumnName("created_on").HasColumnType("timestaptz");
            builder.Property(u => u.ModifiedOn).HasColumnName("modified_on").HasColumnType("timestaptz");
            builder.Property(u => u.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        }
    }
}
