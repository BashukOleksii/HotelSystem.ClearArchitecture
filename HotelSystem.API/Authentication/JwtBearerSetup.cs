using HotelSystem.API.Errors;
using HotelSystem.Infrastructure.Security.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HotelSystem.API.Authentication;

internal static class JwtBearerSetup
{
    public static IServiceCollection AddApiJwt(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((bearer, jwt) =>
            {
                var config = jwt.Value;
                bearer.MapInboundClaims = false;
                bearer.IncludeErrorDetails = false;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(config.SigningKeyBase64)),
                    ValidateIssuer = true, ValidIssuer = config.Issuer,
                    ValidateAudience = true, ValidAudience = config.Audience,
                    ValidateLifetime = true, RequireExpirationTime = true, RequireSignedTokens = true,
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256], ValidTypes = ["at+jwt"],
                    ClockSkew = TimeSpan.FromSeconds(30), NameClaimType = "unique_name", RoleClaimType = "role"
                };
                bearer.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (!Guid.TryParse(context.Principal?.FindFirst("sub")?.Value, out var id) || id == Guid.Empty)
                            context.Fail("Invalid subject.");
                        return Task.CompletedTask;
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse(); context.Response.Headers.WWWAuthenticate = "Bearer";
                        await ApiProblems.WriteAsync(context.HttpContext,
                            ApiProblems.Create(context.HttpContext, 401, "Потрібна автентифікація."), context.HttpContext.RequestAborted);
                    },
                    OnForbidden = context => ApiProblems.WriteAsync(context.HttpContext,
                        ApiProblems.Create(context.HttpContext, 403, "Недостатньо прав для виконання операції."), context.HttpContext.RequestAborted)
                };
            });
        services.AddAuthorization(options => options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().Build());
        return services;
    }
}
