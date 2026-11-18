using HotelSystem.Application.Features.Users.DTOs;

namespace HotelSystem.Application.Common.Interfaces;

public interface IAccessTokenIssuer
{
    Task<AccessTokenDto> IssueAsync(Guid userId, string username, IReadOnlyCollection<string> roles,
        CancellationToken cancellationToken = default);
}
