using HotelSystem.Application.Common.Models;
namespace HotelSystem.Application.Common.Interfaces;
public interface IPhotoStorage
{
    Task<StoredPhoto> UploadAsync(PhotoUpload upload, CancellationToken cancellationToken = default);
}
