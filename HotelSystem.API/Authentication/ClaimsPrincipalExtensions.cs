using System.Security.Claims;
using HotelSystem.Application.Common.Exceptions;

namespace HotelSystem.API.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid RequiredUserId(this ClaimsPrincipal principal) =>
        Guid.TryParse(principal.FindFirst("sub")?.Value, out var id) && id != Guid.Empty
            ? id : throw new AuthenticationFailedException();
}
