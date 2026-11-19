using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Infrastructure.Persistence;
using HotelSystem.Infrastructure.Persistence.Outbox;
using HotelSystem.Infrastructure.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HotelSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddHotelPersistence(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        if (new SqlConnectionStringBuilder(connectionString).MultipleActiveResultSets)
            throw new ArgumentException("Вимкніть MultipleActiveResultSets для передбачуваних транзакцій і savepoints.", nameof(connectionString));
        
        services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(connectionString));

        services.AddLogging(); 
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IHotelRepository, HotelRepository>(); services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>(); services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUserRepository, UserRepository>(); services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>(); services.AddScoped<IPhotoCleanupQueue, EfPhotoCleanupQueue>();

        return services;
    }
}
