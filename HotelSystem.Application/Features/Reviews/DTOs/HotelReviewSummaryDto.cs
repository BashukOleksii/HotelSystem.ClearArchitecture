namespace HotelSystem.Application.Features.Reviews.DTOs;

public sealed record HotelReviewSummaryDto(Guid HotelId, long ReviewCount, decimal? AverageRating);
