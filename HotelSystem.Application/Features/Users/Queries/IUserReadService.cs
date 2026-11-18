using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Users.DTOs;

namespace HotelSystem.Application.Features.Users.Queries;

public interface IUserReadService
{
    Task<PagedResult<UserListItemDto>> SearchAsync(UserFilter filter, CancellationToken cancellationToken = default);
}
