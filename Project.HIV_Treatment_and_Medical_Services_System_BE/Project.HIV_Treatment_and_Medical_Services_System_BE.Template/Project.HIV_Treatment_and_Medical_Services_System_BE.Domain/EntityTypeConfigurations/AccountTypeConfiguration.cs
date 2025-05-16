using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities;

namespace Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.EntityTypeConfigurations
{
    /// <summary>
    /// Account configuration.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration&lt;Project.HIV_Treatment_and_Medical_Services_System_BE.Domain.Entities.Account&gt;" />
    public class AccountTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Primary Key
            builder.HasKey(e => e.Id);

            // Config the column.
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("uuid");
            builder.Property(e => e.UserName).HasColumnName("user_name").HasColumnType("varchar").HasMaxLength(20);
            builder.Property(e => e.Password).HasColumnName("password").HasColumnType("varchar").HasMaxLength(20);
            builder.Property(e => e.Status).HasColumnName("status").HasColumnType("varchar").HasMaxLength(20);
            builder.Property(e => e.UserId).HasColumnName("user_id").HasColumnType("uuid");

            // Default base entity property column.
            builder.Property(e => e.CreatedOn).HasColumnName("created_on").HasColumnType("timestaptz");
            builder.Property(e => e.ModifiedOn).HasColumnName("modified_on").HasColumnType("timestaptz");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            // Foregin Key
            builder.HasOne(a => a.User)
            .WithOne(u => u.Account)
            .HasForeignKey<User>(u => u.Id)
            .IsRequired(false);
        }
    }
}
