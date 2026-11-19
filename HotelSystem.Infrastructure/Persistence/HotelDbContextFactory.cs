using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HotelSystem.Infrastructure.Persistence;

public sealed class HotelDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
{
    public HotelDbContext CreateDbContext(string[] args)
    {
        var connection = Environment.GetEnvironmentVariable("HOTELSYSTEM_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Встановіть HOTELSYSTEM_CONNECTION_STRING для окремої БД HotelSystemClean перед запуском EF tools.");
        var options = new DbContextOptionsBuilder<HotelDbContext>().UseSqlServer(connection).Options;
        return new HotelDbContext(options);
    }
}
