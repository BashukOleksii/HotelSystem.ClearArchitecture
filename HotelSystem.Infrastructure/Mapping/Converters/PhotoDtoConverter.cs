using AutoMapper;
using HotelSystem.Application.Common.Models;
using HotelSystem.Infrastructure.DTOs.Photos;
using HotelSystem.Infrastructure.Media;

namespace HotelSystem.Infrastructure.Mapping.Converters;

internal sealed class PhotoDtoConverter(PhotoUrlBuilder urls) : ITypeConverter<PhotoReadData, PhotoDto>
{
    public PhotoDto Convert(PhotoReadData source, PhotoDto destination, ResolutionContext context) =>
        new(source.Id, urls.Build(source.StorageKey));
}
