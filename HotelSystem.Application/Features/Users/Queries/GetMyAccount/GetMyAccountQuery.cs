using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Queries.GetMyAccount;

public sealed record GetMyAccountQuery(Guid ActorId) : IRequest<UserAccountDto>;
