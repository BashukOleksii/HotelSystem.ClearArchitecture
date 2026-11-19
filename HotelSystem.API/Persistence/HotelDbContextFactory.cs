using HotelSystem.API.Configuration;
using HotelSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HotelSystem.API.Persistence;

// EF tooling reads the same files as the API without starting workers or bootstrap seeding.
public sealed class HotelDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
{
    public HotelDbContext CreateDbContext(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = FindContentRoot(),
            EnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environments.Development
        });
        builder.Configuration.AddLocalDevelopmentConfiguration(builder.Environment, args);

        var connection = builder.Configuration.GetConnectionString("HotelDatabase");
        if (string.IsNullOrWhiteSpace(connection))
            connection = Environment.GetEnvironmentVariable("HOTELSYSTEM_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Налаштуйте ConnectionStrings:HotelDatabase у локальному файлі API або через змінні середовища.");

        return new HotelDbContext(new DbContextOptionsBuilder<HotelDbContext>().UseSqlServer(connection).Options);
    }

    private static string FindContentRoot()
    {
        for (var directory = new DirectoryInfo(Directory.GetCurrentDirectory()); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "HotelSystem.API.csproj"))) return directory.FullName;
            var apiDirectory = Path.Combine(directory.FullName, "HotelSystem.API");
            if (File.Exists(Path.Combine(apiDirectory, "HotelSystem.API.csproj"))) return apiDirectory;
        }

        return AppContext.BaseDirectory;
    }
}
