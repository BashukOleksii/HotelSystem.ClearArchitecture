using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> b)
    {
        b.ToTable("Rooms", t =>
        {
            t.HasCheckConstraint("CK_Rooms_Type", "[Type] IN (1,2,3,4,5)");
            t.HasCheckConstraint("CK_Rooms_Capacity", "[Capacity] >= 1 AND [Capacity] <= CASE [Type] WHEN 1 THEN 1 WHEN 2 THEN 2 WHEN 3 THEN 2 WHEN 4 THEN 4 WHEN 5 THEN 5 ELSE 0 END");
            t.HasCheckConstraint("CK_Rooms_Price", "[CostPerNight] > 0");
        });
        b.HasKey(x => x.Id); b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.RoomNumber).HasMaxLength(20).UseCollation(IdentityNormalization.RoomNumberCollation).IsRequired();
        b.Property(x => x.CostPerNight).HasPrecision(18, 2);
        b.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedNever();
        b.HasOne<Hotel>().WithMany().HasForeignKey(x => x.HotelId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(x => new { x.HotelId, x.RoomNumber }).IsUnique().HasFilter("[IsDeleted] = 0").HasDatabaseName("UX_Rooms_HotelId_RoomNumber");
        b.OwnsMany(x => x.Photos, p =>
        {
            p.ToTable("RoomPhotos"); p.WithOwner().HasForeignKey("RoomId");
            p.HasKey(x => x.Id); p.Property(x => x.Id).ValueGeneratedNever();
            p.Property(x => x.StorageKey).HasMaxLength(512).IsRequired(); p.HasIndex(x => x.StorageKey);
        });
        b.Navigation(x => x.Photos).HasField("_photos").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
