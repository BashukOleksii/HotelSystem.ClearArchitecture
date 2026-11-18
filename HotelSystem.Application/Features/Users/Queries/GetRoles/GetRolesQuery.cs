using HotelSystem.Application.Common.Models;
using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Queries.GetRoles;

public sealed record GetRolesQuery(Guid ActorId) : IRequest<IReadOnlyList<string>>;
