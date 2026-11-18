namespace HotelSystem.Application.Common.Models;
public sealed record PhotoUpload(Stream Content, string FileName, string ContentType)
{
    public const long MaximumBytes = 5 * 1024 * 1024;
}
public sealed record StoredPhoto(string StorageKey, string Url);
