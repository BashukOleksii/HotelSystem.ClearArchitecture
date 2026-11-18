using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Domain.ValueObjects;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.UpdateHotel;

public sealed class UpdateHotelCommandHandler(
    IUserRepository users,
    IHotelRepository hotels,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateHotelCommand>
{
    public async Task Handle(
        UpdateHotelCommand request,
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
                "Зміна готелю доступна активному користувачу з роллю Owner.");
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
                "Ви можете змінювати лише власні готелі.");
        }

        var address = Address.Create(
            request.City,
            request.Street,
            request.Number);

        hotel.UpdateDetails(
            request.Name,
            address,
            request.Description);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}