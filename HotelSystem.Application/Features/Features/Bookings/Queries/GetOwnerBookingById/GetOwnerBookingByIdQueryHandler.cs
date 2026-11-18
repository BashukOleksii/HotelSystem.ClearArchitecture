using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Bookings.Common;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookingById;

public sealed class GetOwnerBookingByIdQueryHandler(IUserRepository users, IBookingReadService reads)
    : IRequestHandler<GetOwnerBookingByIdQuery, BookingManagementDto>
{
    public async Task<BookingManagementDto> Handle(GetOwnerBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var actor = await BookingAccess.RequireRoleAsync(request.ActorId, RoleNames.Owner, users, cancellationToken);
        var result = await reads.GetOwnerByIdAsync(actor.Id, request.BookingId, cancellationToken);
        return result ?? throw new NotFoundException("Бронювання не знайдено.");
    }
}
