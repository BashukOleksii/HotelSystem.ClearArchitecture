using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Features.Hotels.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Hotels.Queries.GetHotelById;

public sealed class GetHotelByIdQueryHandler(
    IHotelReadService hotelReads)
    : IRequestHandler<GetHotelByIdQuery, HotelDetailsDto>
{
    public async Task<HotelDetailsDto> Handle(
        GetHotelByIdQuery request,
        CancellationToken cancellationToken)
    {
        var hotel = await hotelReads.GetApprovedByIdAsync(
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