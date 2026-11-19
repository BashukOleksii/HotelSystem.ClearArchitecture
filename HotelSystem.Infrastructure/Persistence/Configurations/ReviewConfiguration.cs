using HotelSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> b)
    {
        b.ToTable("Reviews", t =>
        {
            t.HasCheckConstraint("CK_Reviews_Rating", "[Rating] BETWEEN 1 AND 5");
            t.HasCheckConstraint("CK_Reviews_DeleteAudit", "([IsDeleted] = 0 AND [DeletedAt] IS NULL AND [DeletedByUserId] IS NULL) OR ([IsDeleted] = 1 AND [DeletedAt] IS NOT NULL AND [DeletedByUserId] IS NOT NULL)");
        });
        b.HasKey(x => x.Id); b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.Comment).HasMaxLength(2000);
        b.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedNever();
        b.HasOne<Hotel>().WithMany().HasForeignKey(x => x.HotelId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<User>().WithMany().HasForeignKey(x => x.DeletedByUserId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(x => new { x.HotelId, x.UserId }).IsUnique().HasFilter("[IsDeleted] = 0").HasDatabaseName("UX_Reviews_HotelId_UserId");
        b.HasIndex(x => new { x.UserId, x.CreatedAt });
        b.OwnsMany(x => x.Photos, p =>
        {
            p.ToTable("ReviewPhotos"); p.WithOwner().HasForeignKey("ReviewId");
            p.HasKey(x => x.Id); p.Property(x => x.Id).ValueGeneratedNever();
            p.Property(x => x.StorageKey).HasMaxLength(512).IsRequired(); p.HasIndex(x => x.StorageKey);
        });
        b.Navigation(x => x.Photos).HasField("_photos").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
