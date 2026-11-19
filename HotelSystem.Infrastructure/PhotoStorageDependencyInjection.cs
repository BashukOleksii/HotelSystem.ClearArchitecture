using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Infrastructure.Media;
using HotelSystem.Infrastructure.Persistence.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HotelSystem.Infrastructure;

public static class PhotoStorageDependencyInjection
{
    public static IServiceCollection AddHotelPhotoStorage(this IServiceCollection services, Action<PhotoStorageOptions>? configure = null)
    {
        services.AddLogging(); services.TryAddSingleton<TimeProvider>(TimeProvider.System);
        var options = services.AddOptions<PhotoStorageOptions>();
        if (configure is not null) options.Configure(configure);
        options.Validate(x => x.IsValid(), "Некоректні налаштування PhotoStorage.").ValidateOnStart();
        services.AddHotelReadServices();
        services.TryAddSingleton<IPhotoStorage, LocalPhotoStorage>();
        services.TryAddScoped<PhotoCleanupProcessor>();
        services.AddHostedService<PhotoCleanupWorker>();
        return services;
    }
}
