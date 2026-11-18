using HotelSystem.Application.Features.Reviews.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Reviews.Queries.GetHotelReviewSummary;

public sealed record GetHotelReviewSummaryQuery(Guid HotelId) : IRequest<HotelReviewSummaryDto>;
