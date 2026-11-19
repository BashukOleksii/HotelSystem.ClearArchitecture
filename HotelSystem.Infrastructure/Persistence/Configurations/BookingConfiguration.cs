using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> b)
    {
        b.ToTable("Bookings", t =>
        {
            t.HasCheckConstraint("CK_Bookings_Dates", "[CheckOut] > [CheckIn]");
            t.HasCheckConstraint("CK_Bookings_Status", "[Status] IN (1,2)");
            t.HasCheckConstraint("CK_Bookings_Prices", "[PricePerNight] > 0 AND [TotalPrice] > 0");
        });
        b.HasKey(x => x.Id); b.Property(x => x.Id).ValueGeneratedNever();
        b.Ignore(x => x.Nights);
        b.Property(x => x.PricePerNight).HasPrecision(18, 2); b.Property(x => x.TotalPrice).HasPrecision(18, 2);
        b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<Room>().WithMany().HasForeignKey(x => x.RoomId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(x => new { x.RoomId, x.Status, x.CheckIn, x.CheckOut });
        b.HasIndex(x => new { x.UserId, x.CreatedAt });
    }
}
