using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Entities;
using HotelSystem.Infrastructure.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Infrastructure.Persistence;

public sealed class HotelDbContext(DbContextOptions<HotelDbContext> options) : DbContext(options)
{
    private bool _faulted;
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<OwnerProfile> OwnerProfiles => Set<OwnerProfile>();
    public DbSet<AdminProfile> AdminProfiles => Set<AdminProfile>();
    public DbSet<PhotoCleanupJob> PhotoCleanupJobs => Set<PhotoCleanupJob>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelDbContext).Assembly);
    }

    internal void EnsureUsable()
    {
        if (_faulted) throw new InvalidOperationException("Цю одиницю роботи завершено помилкою. Створіть новий scope для повторної операції.");
    }

    internal void MarkFaulted()
    {
        _faulted = true;
        ChangeTracker.Clear();
    }

    private void PrepareChanges()
    {
        EnsureUsable();
        ChangeTracker.DetectChanges();
        foreach (var entry in ChangeTracker.Entries<User>().Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            entry.Property("NormalizedUsername").CurrentValue = IdentityNormalization.Key(entry.Entity.Username);
            entry.Property("NormalizedEmail").CurrentValue = IdentityNormalization.Key(entry.Entity.Email);
        }
        foreach (var entry in ChangeTracker.Entries<Role>().Where(e => e.State is EntityState.Added or EntityState.Modified))
            entry.Property("NormalizedName").CurrentValue = IdentityNormalization.Key(entry.Entity.Name);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        PrepareChanges();
        try { return base.SaveChanges(acceptAllChangesOnSuccess); }
        catch (Exception error)
        {
            MarkFaulted();
            if (PersistenceErrors.Translate(error) is { } translated) throw translated;
            throw;
        }
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PrepareChanges();
        try { return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken); }
        catch (Exception error)
        {
            MarkFaulted();
            if (PersistenceErrors.Translate(error) is { } translated) throw translated;
            throw;
        }
    }
}
