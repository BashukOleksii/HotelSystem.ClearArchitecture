using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyAccount;

public sealed class UpdateMyAccountCommandHandler(IUserRepository users, IPasswordHasher passwords, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMyAccountCommand>
{
    public async Task Handle(UpdateMyAccountCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var user = await UserWorkflow.RequireAsync(request.ActorId, RoleNames.User, users, ct);
            if (!passwords.Verify(request.CurrentPassword, user.PasswordHash)) throw new AuthenticationFailedException();
            await UserWorkflow.EnsureUniqueAsync(request.Details, user.Id, users, ct);
            user.UpdateProfile(request.Details.Username, request.Details.Email, request.Details.PhoneNumber);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
