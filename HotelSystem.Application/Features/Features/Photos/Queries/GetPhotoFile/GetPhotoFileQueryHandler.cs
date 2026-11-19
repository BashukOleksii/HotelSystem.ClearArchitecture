using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Common.Models;
using MediatR;
namespace HotelSystem.Application.Features.Photos.Queries.GetPhotoFile;

public sealed class GetPhotoFileQueryHandler(IPhotoReferenceReader references, IPhotoStorage storage) : IRequestHandler<GetPhotoFileQuery, PhotoFile>
{
    public async Task<PhotoFile> Handle(GetPhotoFileQuery request, CancellationToken cancellationToken)
    {
        if (!await references.ExistsAsync(request.StorageKey, cancellationToken)) throw new NotFoundException("Фотографію не знайдено.");
        return await storage.OpenReadAsync(request.StorageKey, cancellationToken) ?? throw new NotFoundException("Файл фотографії не знайдено.");
    }
}
