using System.ComponentModel.DataAnnotations;

namespace HotelSystem.API.Contracts.Photos;

public sealed class PhotoUploadRequest
{
    [Required]
    public IFormFile File { get; set; } = null!;
}
