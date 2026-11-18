using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetMyHotelById;

public sealed class GetMyHotelByIdQueryHandler(
    IUserRepository users,
    IHotelReadService hotelReads)
    : IRequestHandler<GetMyHotelByIdQuery, HotelManagementDetailsDto>
{
    public async Task<HotelManagementDetailsDto> Handle(
        GetMyHotelByIdQuery request,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetByIdAsync(
            request.ActorId,
            cancellationToken);

        if (actor is null
            || actor.IsBlocked
            || !actor.HasRole(RoleNames.Owner))
        {
            throw new ForbiddenOperationException(
                "Перегляд власного готелю доступний активному користувачу з роллю Owner.");
        }

        var hotel = await hotelReads.GetOwnerByIdAsync(
            ownerId: actor.Id,
            id: request.HotelId,
            cancellationToken: cancellationToken);

        if (hotel is null)
        {
            throw new NotFoundException(
                "Готель не знайдено.");
        }

        return hotel;
    }
}