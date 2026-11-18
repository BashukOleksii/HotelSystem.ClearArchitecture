using HotelSystem.Application.Features.Users.DTOs;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyAdminProfile;

public sealed record UpdateMyAdminProfileCommand(Guid ActorId, string? EmployeeNumber = null, string? DepartmentName = null) : IRequest;
