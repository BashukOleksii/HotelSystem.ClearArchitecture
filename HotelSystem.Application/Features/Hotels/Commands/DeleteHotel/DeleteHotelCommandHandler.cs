using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.DeleteHotel;

public sealed class DeleteHotelCommandHandler(
    IUserRepository users,
    IHotelRepository hotels,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteHotelCommand>
{
    public async Task Handle(
        DeleteHotelCommand request,
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
                "Видалення готелю доступне активному користувачу з роллю Owner.");
        }

        var hotel = await hotels.GetByIdAsync(
            request.HotelId,
            cancellationToken);

        if (hotel is null)
        {
            throw new NotFoundException(
                "Готель не знайдено.");
        }

        if (hotel.OwnerId != actor.Id)
        {
            throw new ForbiddenOperationException(
                "Ви можете видаляти лише власні готелі.");
        }

        hotel.Delete();

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}