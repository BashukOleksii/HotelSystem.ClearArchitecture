using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(IUserRepository users, IRoleRepository roles, IPasswordHasher passwords, IUnitOfWork unitOfWork, TimeProvider clock)
    : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var hash = passwords.Hash(request.Password);
        Guid id = default;
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
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
