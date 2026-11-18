using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Exceptions;

namespace HotelSystem.Domain.Services;

internal static class PhotoGalleryService
{
    public static Photo Add(
        ICollection<Photo> photos,
        string storageKey,
        int maximumCount)
    {
        if (photos.Count >= maximumCount)
        {
            throw new DomainValidationException(
                nameof(photos),
                $"Галерея може містити максимум {maximumCount} фотографій.");
        }

        var photo = Photo.Create(storageKey);

        if (photos.Any(existing => existing.StorageKey == storageKey))
        {
            throw new DomainValidationException(
                nameof(storageKey),
                "Цю фотографію вже додано до галереї.");
        }

        photos.Add(photo);

        return photo;
    }

    public static Photo Remove(
        ICollection<Photo> photos,
        Guid photoId)
    {
        var photo = photos.FirstOrDefault(
            existing => existing.Id == photoId);

        if (photo is null)
        {
            throw new DomainValidationException(
                nameof(photoId),
                "Фотографія не належить цій галереї.");
        }

        photos.Remove(photo);

        return photo;
    }
}