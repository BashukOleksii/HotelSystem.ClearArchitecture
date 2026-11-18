using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Bookings.Common;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetMyBookingById;

public sealed class GetMyBookingByIdQueryHandler(IUserRepository users, IBookingReadService reads)
    : IRequestHandler<GetMyBookingByIdQuery, BookingDetailsDto>
{
    public async Task<BookingDetailsDto> Handle(GetMyBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var actor = await BookingAccess.RequireRoleAsync(request.ActorId, RoleNames.User, users, cancellationToken);
        var result = await reads.GetClientByIdAsync(actor.Id, request.BookingId, cancellationToken);
        return result ?? throw new NotFoundException("Бронювання не знайдено.");
    }
}
