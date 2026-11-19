using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class OwnerProfileConfiguration : IEntityTypeConfiguration<OwnerProfile>
{
    public void Configure(EntityTypeBuilder<OwnerProfile> b)
    {
        b.ToTable("OwnerProfiles"); b.HasKey(x => x.UserId); b.Property(x => x.UserId).ValueGeneratedNever();
        b.HasOne<User>().WithOne().HasForeignKey<OwnerProfile>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.Property(x => x.CompanyName).HasMaxLength(150);
        b.Property(x => x.BusinessPhone).HasMaxLength(30);
    }
}
