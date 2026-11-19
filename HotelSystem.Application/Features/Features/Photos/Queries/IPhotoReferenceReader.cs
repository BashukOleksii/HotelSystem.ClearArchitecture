namespace HotelSystem.Application.Features.Photos.Queries;

public interface IPhotoReferenceReader
{
    Task<bool> ExistsAsync(string storageKey, CancellationToken cancellationToken = default);
}
