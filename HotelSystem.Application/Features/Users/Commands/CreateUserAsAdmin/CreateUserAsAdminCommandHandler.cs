using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.CreateUserAsAdmin;

public sealed class CreateUserAsAdminCommandHandler(IUserRepository users, IRoleRepository roles, IPasswordHasher passwords, IUnitOfWork unitOfWork, TimeProvider clock)
    : IRequestHandler<CreateUserAsAdminCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserAsAdminCommand request, CancellationToken cancellationToken)
    {
        await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        var hash = passwords.Hash(request.Password);
        Guid id = default;
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, ct);
            await UserWorkflow.EnsureUniqueAsync(request.Details, null, users, ct);
            var role = await roles.GetByNameAsync(RoleNames.User, ct)
                ?? throw new NotFoundException("Базова роль User не налаштована.");
            var user = User.Create(request.Details.Username, request.Details.Email, hash, role,
                clock.GetUtcNow(), request.Details.PhoneNumber);
            users.Add(user);
            await unitOfWork.SaveChangesAsync(ct);
            id = user.Id;
        }, cancellationToken);
        return id;
    }
}
