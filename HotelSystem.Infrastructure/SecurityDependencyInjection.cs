using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Infrastructure.Persistence.Seeding;
using HotelSystem.Infrastructure.Security.Passwords;
using HotelSystem.Infrastructure.Security.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace HotelSystem.Infrastructure;

public static class SecurityDependencyInjection
{
    public static IServiceCollection AddHotelSecurity(this IServiceCollection services, Action<JwtOptions> configureJwt,
        Action<PasswordHashingOptions>? configurePasswords = null, Action<BootstrapAdminOptions>? configureBootstrap = null)
    {
        ArgumentNullException.ThrowIfNull(configureJwt);
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);

        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>());
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<BootstrapAdminOptions>, BootstrapAdminOptionsValidator>());
        services.AddOptions<JwtOptions>().Configure(configureJwt).ValidateOnStart();
        var passwordOptions = services.AddOptions<PasswordHashingOptions>();
        if (configurePasswords is not null) passwordOptions.Configure(configurePasswords);
        passwordOptions.Validate(x => x.WorkFactor is >= PasswordHashingOptions.MinimumWorkFactor and <= PasswordHashingOptions.MaximumWorkFactor,
            "PasswordHashing:WorkFactor має бути від 10 до 16.").ValidateOnStart();
        var bootstrapOptions = services.AddOptions<BootstrapAdminOptions>();
        if (configureBootstrap is not null) bootstrapOptions.Configure(configureBootstrap);
        bootstrapOptions.ValidateOnStart();

        services.TryAddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.TryAddSingleton<IAccessTokenIssuer, JwtAccessTokenIssuer>();
        services.TryAddScoped<BootstrapAdminSeeder>();
        services.TryAddScoped<DemoDataSeeder>();
        return services;
    }
}
