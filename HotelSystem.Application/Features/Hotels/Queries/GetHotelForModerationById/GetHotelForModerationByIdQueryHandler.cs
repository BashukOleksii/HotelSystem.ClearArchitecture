using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelForModerationById;

public sealed class GetHotelForModerationByIdQueryHandler(
    IUserRepository users,
    IHotelReadService hotelReads)
    : IRequestHandler<GetHotelForModerationByIdQuery, HotelManagementDetailsDto>
{
    public async Task<HotelManagementDetailsDto> Handle(
        GetHotelForModerationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetByIdAsync(
            request.ActorId,
            cancellationToken);

        if (actor is null
            || actor.IsBlocked
            || !actor.HasRole(RoleNames.Admin))
        {
            throw new ForbiddenOperationException(
                "Перегляд готелю для модерації доступний активному користувачу з роллю Admin.");
        }

        var hotel = await hotelReads.GetForModerationByIdAsync(
            request.HotelId,
            cancellationToken);

        if (hotel is null)
        {
            throw new NotFoundException(
                "Готель не знайдено.");
        }

        return hotel;
    }
}