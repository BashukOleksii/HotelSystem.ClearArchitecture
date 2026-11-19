using AutoMapper;
using HotelSystem.Application.Features.Photos.Queries;
using HotelSystem.Application.Features.Hotels.Queries;
using HotelSystem.Application.Features.Rooms.Queries;
using HotelSystem.Application.Features.Bookings.Queries;
using HotelSystem.Application.Features.Reviews.Queries;
using HotelSystem.Application.Features.Users.Queries;
using HotelSystem.Infrastructure.Mapping;
using HotelSystem.Infrastructure.Media;
using HotelSystem.Infrastructure.Persistence.ReadServices;
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

        services.AddHotelReadServices();
        return services;
    }

    public static IServiceCollection AddHotelReadServices(this IServiceCollection services, Action<PhotoUrlOptions>? configure = null)
    {
        services.AddLogging();
        var options = services.AddOptions<PhotoUrlOptions>();
        if (configure is not null) options.Configure(configure);
        options.Validate(x => x.IsValid(), "Некоректна конфігурація URL фотографій.");
        services.TryAddSingleton<PhotoUrlBuilder>();
        if (!services.Any(x => x.ServiceType == typeof(ReadMappingRegistration)))
        {
            services.AddSingleton(new ReadMappingRegistration());
            services.AddAutoMapper(cfg =>
            {
                var key = Environment.GetEnvironmentVariable("AUTOMAPPER_LICENSE_KEY");
                if (!string.IsNullOrWhiteSpace(key)) cfg.LicenseKey = key;
            }, typeof(ReadMappingProfile).Assembly);
        }
        services.TryAddScoped<IHotelReadService, HotelReadService>();
        services.TryAddScoped<IRoomReadService, RoomReadService>();
        services.TryAddScoped<IBookingReadService, BookingReadService>();
        services.TryAddScoped<IReviewReadService, ReviewReadService>();
        services.TryAddScoped<IUserReadService, UserReadService>();
        services.TryAddScoped<IPhotoReferenceReader, PhotoReferenceReader>();
        return services;
    }

    private sealed class ReadMappingRegistration { }
}
