using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class AdminProfileConfiguration : IEntityTypeConfiguration<AdminProfile>
{
    public void Configure(EntityTypeBuilder<AdminProfile> b)
    {
        b.ToTable("AdminProfiles"); b.HasKey(x => x.UserId); b.Property(x => x.UserId).ValueGeneratedNever();
        b.HasOne<User>().WithOne().HasForeignKey<AdminProfile>(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.Property(x => x.EmployeeNumber).HasMaxLength(50);
        b.Property(x => x.DepartmentName).HasMaxLength(100);
    }
}
