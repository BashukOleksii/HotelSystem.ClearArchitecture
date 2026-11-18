namespace HotelSystem.Domain.Rules;

public static class ReviewRules
{
    public const int MinimumRating = 1;
    public const int MaximumRating = 5;
    public const int MaximumCommentLength = 2000;
    public const int MaximumPhotoCount = 5;
}
