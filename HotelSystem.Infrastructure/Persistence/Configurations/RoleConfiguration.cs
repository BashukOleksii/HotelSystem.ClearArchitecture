using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> b)
    {
        b.ToTable("Roles"); b.HasKey(x => x.Id); b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Name).HasMaxLength(50).IsRequired();
        b.Property<string>("NormalizedName").HasMaxLength(50).UseCollation(IdentityNormalization.OrdinalCollation).IsRequired();
        b.HasIndex("NormalizedName").IsUnique();
        b.HasData(
            new { Id = Guid.Parse("cfe8756d-42e4-476e-b742-4ba606bf0001"), Name = RoleNames.User, NormalizedName = "USER" },
            new { Id = Guid.Parse("cfe8756d-42e4-476e-b742-4ba606bf0002"), Name = RoleNames.Owner, NormalizedName = "OWNER" },
            new { Id = Guid.Parse("cfe8756d-42e4-476e-b742-4ba606bf0003"), Name = RoleNames.Admin, NormalizedName = "ADMIN" });
    }
}
