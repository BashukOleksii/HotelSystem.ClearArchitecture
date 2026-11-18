using HotelSystem.Domain.Exceptions;
using HotelSystem.Domain.Services;

namespace HotelSystem.Domain.Entities;

public sealed class Review
{
    public Guid Id { get; private set; }
    public Guid HotelId { get; private set; }
    public Guid UserId { get; private set; }
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private const int MaximumPhotoCount = 5;
    private readonly List<Photo> _photos = [];
    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();

    private Review() { }

    public static Review Create(
        Guid hotelId,
        Guid userId,
        int rating,
        string? comment,
        DateTimeOffset createdAt)
    {
        if (hotelId == Guid.Empty)
        {
            throw new DomainValidationException(
                nameof(hotelId),
                "Вкажіть ідентифікатор готелю.");
        }

        if (userId == Guid.Empty)
        {
            throw new DomainValidationException(
                nameof(userId),
                "Вкажіть автора відгуку.");
        }

        var review = new Review
        {
            Id = Guid.NewGuid(),
            HotelId = hotelId,
            UserId = userId,
            CreatedAt = createdAt
        };

        review.UpdateDetails(rating, comment);

        return review;
    }

    public void UpdateDetails(int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
        {
            throw new DomainValidationException(
                nameof(rating),
                "Оцінка має бути від 1 до 5.");
        }

        var normalizedComment = comment?.Trim();

        if (normalizedComment?.Length > 2000)
        {
            throw new DomainValidationException(
                nameof(comment),
                "Коментар має містити до 2000 символів.");
        }

        Rating = rating;
        Comment = string.IsNullOrEmpty(normalizedComment)
            ? null
            : normalizedComment;
    }

    public Photo AddPhoto(string storageKey)
    {
        return PhotoGalleryService.Add(
            _photos,
            storageKey,
            MaximumPhotoCount);
    }

    public Photo RemovePhoto(Guid photoId)
    {
        return PhotoGalleryService.Remove(_photos, photoId);
    }

}