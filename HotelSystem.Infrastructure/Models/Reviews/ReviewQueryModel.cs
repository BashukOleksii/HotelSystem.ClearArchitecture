using HotelSystem.Domain.Entities;

namespace HotelSystem.Infrastructure.Models.Reviews;

internal sealed class ReviewQueryModel
{
    public Review Review { get; init; } = null!;
    public Hotel Hotel { get; init; } = null!;
    public User User { get; init; } = null!;
}
