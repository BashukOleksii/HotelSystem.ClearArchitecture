using HotelSystem.Domain.Exceptions;
using HotelSystem.Domain.Rules;
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
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public Guid? DeletedByUserId { get; private set; }
    public Guid Version { get; private set; }
    private readonly List<Photo> _photos = [];
    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();

    private Review() { }

    public static Review Create(Guid hotelId, Guid userId, int rating,
        string? comment, DateTimeOffset createdAt)
    {
        if (hotelId == Guid.Empty)
            throw new DomainValidationException(nameof(hotelId), "Вкажіть готель.");
        if (userId == Guid.Empty)
            throw new DomainValidationException(nameof(userId), "Вкажіть автора відгуку.");
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
        EnsureNotDeleted();
        if (rating < ReviewRules.MinimumRating || rating > ReviewRules.MaximumRating)
            throw new DomainValidationException(nameof(rating), "Оцінка має бути від 1 до 5.");
        var text = comment?.Trim();
        if (text?.Length > ReviewRules.MaximumCommentLength)
            throw new DomainValidationException(nameof(comment), "Коментар має містити до 2000 символів.");
        text = string.IsNullOrEmpty(text) ? null : text;
        if (Rating == rating && Comment == text) return;
        Rating = rating;
        Comment = text;
        Touch();
    }

    public void Delete(Guid deletedByUserId, DateTimeOffset deletedAt)
    {
        if (deletedByUserId == Guid.Empty)
            throw new DomainValidationException(nameof(deletedByUserId), "Вкажіть виконавця видалення.");
        if (IsDeleted) return;
        IsDeleted = true;
        DeletedByUserId = deletedByUserId;
        DeletedAt = deletedAt;
        Touch();
    }

    public Photo AddPhoto(string storageKey)
    {
        EnsureNotDeleted();
        var photo = PhotoGalleryService.Add(_photos, storageKey, ReviewRules.MaximumPhotoCount);
        Touch();
        return photo;
    }

    public Photo RemovePhoto(Guid photoId)
    {
        EnsureNotDeleted();
        var photo = PhotoGalleryService.Remove(_photos, photoId);
        Touch();
        return photo;
    }

    private void Touch() => Version = Guid.NewGuid();
    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainValidationException(nameof(IsDeleted), "Видалений відгук не можна змінювати.");
    }
}
