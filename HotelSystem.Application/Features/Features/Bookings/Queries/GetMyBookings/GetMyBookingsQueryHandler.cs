using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.Common;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetMyBookings;

public sealed class GetMyBookingsQueryHandler(IUserRepository users, IBookingReadService reads)
    : IRequestHandler<GetMyBookingsQuery, PagedResult<BookingListItemDto>>
{
    public async Task<PagedResult<BookingListItemDto>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
    {
        var actor = await BookingAccess.RequireRoleAsync(request.ActorId, RoleNames.User, users, cancellationToken);
        return await reads.GetClientListAsync(actor.Id, (request.Filter ?? new BookingFilter()).Normalize(), cancellationToken);
    }
}
