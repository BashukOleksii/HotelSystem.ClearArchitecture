using HotelSystem.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.Infrastructure.Persistence.Configurations;

public sealed class PhotoCleanupJobConfiguration : IEntityTypeConfiguration<PhotoCleanupJob>
{
    public void Configure(EntityTypeBuilder<PhotoCleanupJob> b)
    {
        b.ToTable("PhotoCleanupJobs"); b.HasKey(x => x.Id); b.Property(x => x.Id).ValueGeneratedNever();
        b.Property(x => x.StorageKey).HasMaxLength(512).IsRequired(); b.Property(x => x.LastError).HasMaxLength(1000);
        b.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedNever();
        b.HasIndex(x => new { x.CompletedAt, x.NextAttemptAt });
    }
}
