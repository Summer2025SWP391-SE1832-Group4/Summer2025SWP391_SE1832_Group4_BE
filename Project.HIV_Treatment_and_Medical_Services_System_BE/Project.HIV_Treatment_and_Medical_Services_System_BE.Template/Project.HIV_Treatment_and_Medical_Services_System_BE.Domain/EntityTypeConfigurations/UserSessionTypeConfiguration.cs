using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.EntityTypeConfigurations
{
    /// <summary>
    /// User session configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities.UserSession&gt;" />
    class UserSessionTypeConfiguration : IEntityTypeConfiguration<UserSession>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            // Primary Key
            builder.HasKey(e => e.Id);

            // Config the column.
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid");
            builder.Property(e => e.AccessToken).HasColumnName("user_name").HasColumnType("varchar").HasMaxLength(20);
            builder.Property(e => e.RefereshToken).HasColumnName("password").HasColumnType("varchar").HasMaxLength(20);
            builder.Property(e => e.ExpiredOn).HasColumnName("expired_on").HasColumnType("timestaptz");

            // Default base entity property column.
            builder.Property(e => e.CreatedOn).HasColumnName("created_on").HasColumnType("timestaptz");
            builder.Property(e => e.ModifiedOn).HasColumnName("modified_on").HasColumnType("timestaptz");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            // Foregin Key
            builder.HasOne(us => us.User)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(us => us.UserId)
            .IsRequired(false);
        }
    }
}
