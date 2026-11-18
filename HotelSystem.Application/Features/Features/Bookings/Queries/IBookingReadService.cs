using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Bookings.DTOs;

namespace HotelSystem.Application.Features.Bookings.Queries;

public interface IBookingReadService
{
    Task<PagedResult<BookingListItemDto>> GetClientListAsync(
        Guid userId, BookingFilter filter, CancellationToken ct = default);

    Task<BookingDetailsDto?> GetClientByIdAsync(
        Guid userId, Guid id, CancellationToken ct = default);

    Task<PagedResult<BookingManagementDto>> GetOwnerListAsync(
        Guid ownerId, BookingFilter filter, CancellationToken ct = default);

    Task<BookingManagementDto?> GetOwnerByIdAsync(
        Guid ownerId, Guid id, CancellationToken ct = default);
}
