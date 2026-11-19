using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.ToTable("Users"); b.HasKey(x => x.Id); b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Username).HasMaxLength(50).IsRequired(); b.Property(x => x.Email).HasMaxLength(254).IsRequired();
        b.Property(x => x.PhoneNumber).HasMaxLength(30); b.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
        b.Property<string>("NormalizedUsername").HasMaxLength(50).UseCollation(IdentityNormalization.OrdinalCollation).IsRequired();
        b.Property<string>("NormalizedEmail").HasMaxLength(254).UseCollation(IdentityNormalization.OrdinalCollation).IsRequired();
        b.HasIndex("NormalizedUsername").IsUnique().HasDatabaseName("UX_Users_NormalizedUsername");
        b.HasIndex("NormalizedEmail").IsUnique().HasDatabaseName("UX_Users_NormalizedEmail");
        b.HasIndex(x => x.IsBlocked);
        b.HasMany(x => x.Roles).WithMany().UsingEntity<Dictionary<string, object>>("UserRoles",
            r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Restrict),
            l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
            j => { j.ToTable("UserRoles"); j.HasKey("UserId", "RoleId"); j.HasIndex("RoleId"); });
        b.Navigation(x => x.Roles).HasField("_roles").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
