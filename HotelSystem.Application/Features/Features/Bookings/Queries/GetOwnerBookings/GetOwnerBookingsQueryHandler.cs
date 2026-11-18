using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.Common;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Bookings.Queries.GetOwnerBookings;

public sealed class GetOwnerBookingsQueryHandler(IUserRepository users, IBookingReadService reads)
    : IRequestHandler<GetOwnerBookingsQuery, PagedResult<BookingManagementDto>>
{
    public async Task<PagedResult<BookingManagementDto>> Handle(GetOwnerBookingsQuery request, CancellationToken cancellationToken)
    {
        var actor = await BookingAccess.RequireRoleAsync(request.ActorId, RoleNames.Owner, users, cancellationToken);
        return await reads.GetOwnerListAsync(actor.Id, (request.Filter ?? new BookingFilter()).Normalize(), cancellationToken);
    }
}
