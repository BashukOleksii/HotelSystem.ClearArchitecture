using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> b)
    {
        b.ToTable("Hotels", t => t.HasCheckConstraint("CK_Hotels_ModerationStatus", "[ModerationStatus] IN (1,2,3)"));
        b.HasKey(x => x.Id); b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.Description).HasMaxLength(2000);
        b.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedNever();
        var address = b.ComplexProperty(x => x.Address); address.IsRequired();
        address.Property(x => x.City).HasColumnName("City").HasMaxLength(100).IsRequired();
        address.Property(x => x.Street).HasColumnName("Street").HasMaxLength(100).IsRequired();
        address.Property(x => x.Number).HasColumnName("Number").HasMaxLength(20).IsRequired();
        b.HasOne<User>().WithMany().HasForeignKey(x => x.OwnerId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(x => new { x.OwnerId, x.IsDeleted });
        b.HasIndex(x => new { x.IsDeleted, x.ModerationStatus });
        b.OwnsMany(x => x.Photos, p =>
        {
            p.ToTable("HotelPhotos"); p.WithOwner().HasForeignKey("HotelId");
            p.HasKey(x => x.Id); p.Property(x => x.Id).ValueGeneratedNever();
            p.Property(x => x.StorageKey).HasMaxLength(512).IsRequired(); p.HasIndex(x => x.StorageKey);
        });
        b.Navigation(x => x.Photos).HasField("_photos").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
