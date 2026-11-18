using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using HotelSystem.Domain.ValueObjects;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Commands.CreateHotel;

public sealed class CreateHotelCommandHandler(
    IUserRepository users,
    IHotelRepository hotels,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : IRequestHandler<CreateHotelCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateHotelCommand request,
        CancellationToken cancellationToken)
    {
        var owner = await users.GetByIdAsync(
            request.ActorId,
            cancellationToken);

        if (owner is null
            || owner.IsBlocked
            || !owner.HasRole(RoleNames.Owner))
        {
            throw new ForbiddenOperationException(
                "Створення готелю доступне активному " +
                "користувачу з роллю Owner.");
        }

        var address = Address.Create(
            request.City,
            request.Street,
            request.Number);

        var hotel = Hotel.Create(
            ownerId: owner.Id,
            name: request.Name,
            address: address,
            description: request.Description,
            createdAt: timeProvider.GetUtcNow());

        hotels.Add(hotel);

        await unitOfWork.SaveChangesAsync(
            cancellationToken);

        return hotel.Id;
    }
}