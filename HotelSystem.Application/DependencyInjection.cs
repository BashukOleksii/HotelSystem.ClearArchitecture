using FluentValidation;
using HotelSystem.Application.Common.Time;
using HotelSystem.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HotelSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(
            assembly,
            ServiceLifetime.Scoped);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.TryAddSingleton<TimeProvider>(
            TimeProvider.System);

        services.TryAddSingleton<BookingCalendar>();

        return services;
    }
}