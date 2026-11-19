using HotelSystem.Infrastructure.DTOs.Photos;
using HotelSystem.Infrastructure.DTOs.Hotels;
using HotelSystem.Infrastructure.DTOs.Rooms;
using HotelSystem.Infrastructure.DTOs.Bookings;
using HotelSystem.Infrastructure.DTOs.Reviews;
using HotelSystem.Infrastructure.DTOs.Users;
using HotelSystem.Infrastructure.Mapping.Converters;
using AutoMapper;
using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Hotels.DTOs;
using HotelSystem.Application.Features.Rooms.DTOs;
using HotelSystem.Application.Features.Bookings.DTOs;
using HotelSystem.Application.Features.Reviews.DTOs;
using HotelSystem.Application.Features.Users.DTOs;

namespace HotelSystem.Infrastructure.Mapping;

public sealed class ReadMappingProfile : Profile
{
    public ReadMappingProfile()
    {
        CreateMap<PhotoReadData, PhotoDto>().ConvertUsing<PhotoDtoConverter>();
        CreateMap<HotelReadData, HotelListItemDto>();
        CreateMap<HotelReadData, HotelDetailsDto>();
        CreateMap<HotelReadData, HotelManagementListItemDto>();
        CreateMap<HotelReadData, HotelManagementDetailsDto>();
        CreateMap<RoomReadData, RoomListItemDto>();
        CreateMap<RoomReadData, RoomDetailsDto>();
        CreateMap<RoomReadData, RoomManagementDto>();
        CreateMap<BookingReadData, BookingListItemDto>();
        CreateMap<BookingReadData, BookingDetailsDto>();
        CreateMap<BookingReadData, BookingManagementDto>()
            .ForCtorParam(nameof(BookingManagementDto.Booking), o => o.MapFrom(s => s))
            .ForMember(d => d.Booking, o => o.MapFrom(s => s));
        CreateMap<ReviewReadData, ReviewDto>();
        CreateMap<ReviewReadData, ReviewModerationDto>()
            .ForCtorParam(nameof(ReviewModerationDto.Review), o => o.MapFrom(s => s))
            .ForMember(d => d.Review, o => o.MapFrom(s => s));
        CreateMap<UserReadData, UserListItemDto>();
    }
}
