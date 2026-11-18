using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.ChangeMyPassword;

public sealed class ChangeMyPasswordCommandHandler(IUserRepository users, IPasswordHasher passwords, IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeMyPasswordCommand>
{
    public async Task Handle(ChangeMyPasswordCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var user = await UserWorkflow.RequireAsync(request.ActorId, RoleNames.User, users, ct);
            if (!passwords.Verify(request.CurrentPassword, user.PasswordHash)) throw new AuthenticationFailedException();
            user.ChangePasswordHash(passwords.Hash(request.NewPassword));
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
