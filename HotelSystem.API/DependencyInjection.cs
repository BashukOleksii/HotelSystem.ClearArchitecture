using System.Text.Json;
using System.Text.Json.Serialization;
using HotelSystem.API.Authentication;
using HotelSystem.API.Errors;
using HotelSystem.API.OpenApi;
using HotelSystem.Application;
using HotelSystem.Infrastructure;
using HotelSystem.Infrastructure.Media;
using HotelSystem.Infrastructure.Persistence.Seeding;
using HotelSystem.Infrastructure.Security.Passwords;
using HotelSystem.Infrastructure.Security.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

namespace HotelSystem.API;

public static class DependencyInjection
{
    public static IServiceCollection AddHotelApi(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddApplication();
        var connection = configuration.GetConnectionString("HotelDatabase");
        if (string.IsNullOrWhiteSpace(connection)) connection = Environment.GetEnvironmentVariable("HOTELSYSTEM_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Налаштуйте ConnectionStrings:HotelDatabase або HOTELSYSTEM_CONNECTION_STRING.");
        services.AddHotelPersistence(connection);
        services.AddHotelSecurity(jwt => configuration.GetSection(JwtOptions.SectionName).Bind(jwt),
            passwords => configuration.GetSection(PasswordHashingOptions.SectionName).Bind(passwords),
            bootstrap => configuration.GetSection(BootstrapAdminOptions.SectionName).Bind(bootstrap));
        services.AddHotelPhotoStorage(options =>
        {
            options.RootDirectory = Path.Combine(environment.ContentRootPath, "App_Data", "photos");
            configuration.GetSection(PhotoStorageOptions.SectionName).Bind(options);
        });
        services.Configure<PhotoUrlOptions>(configuration.GetSection("PhotoUrls"));
        services.AddApiJwt();
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddProblemDetails();
        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false)));
        services.Configure<ApiBehaviorOptions>(options => options.InvalidModelStateResponseFactory = action =>
        {
            var problem = new ValidationProblemDetails(action.ModelState)
            { Status = 400, Title = "Некоректний формат запиту.", Instance = action.HttpContext.Request.Path };
            problem.Extensions["traceId"] = action.HttpContext.TraceIdentifier;
            var result = new BadRequestObjectResult(problem); result.ContentTypes.Add("application/problem+json"); return result;
        });

        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
        services.AddCors(options => options.AddPolicy("Angular", policy =>
        {
            if (origins.Length > 0) policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
        }));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelSystem API", Version = "v1", Description = "Готелі, кімнати, бронювання, відгуки, користувачі та фотографії." });
            options.CustomSchemaIds(SchemaId);
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            { Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT", Description = "Вставте accessToken без префікса Bearer." });
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });
            options.OperationFilter<AnonymousOperationFilter>();
        });
        return services;
    }

    private static string SchemaId(Type type) => type.IsArray ? SchemaId(type.GetElementType()!) + "Array"
        : type.IsGenericType ? type.Name.Split('`')[0] + "Of" + string.Join("And", type.GetGenericArguments().Select(SchemaId))
        : (type.FullName ?? type.Name).Replace('+', '.');
}
