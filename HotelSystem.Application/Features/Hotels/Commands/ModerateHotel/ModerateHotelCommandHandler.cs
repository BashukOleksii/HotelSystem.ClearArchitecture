using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Enums;
using HotelSystem.Domain.Exceptions;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.ModerateHotel;

public sealed class ModerateHotelCommandHandler(
    IUserRepository users,
    IHotelRepository hotels,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ModerateHotelCommand>
{
    public async Task Handle(
        ModerateHotelCommand request,
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
                "Модерація готелів доступна активному користувачу з роллю Admin.");
        }

        var hotel = await hotels.GetByIdAsync(
            request.HotelId,
            cancellationToken);

        if (hotel is null || hotel.IsDeleted)
        {
            throw new NotFoundException(
                "Готель не знайдено.");
        }

        switch (request.Status)
        {
            case HotelModerationStatus.Approved:
                hotel.Approve();
                break;

            case HotelModerationStatus.Rejected:
                hotel.Reject();
                break;

            default:
                throw new DomainValidationException(
                    nameof(request.Status),
                    "Рішення модерації має бути Approved або Rejected.");
        }

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}