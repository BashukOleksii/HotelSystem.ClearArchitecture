using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.SetUserBlocked;

public sealed class SetUserBlockedCommandHandler(IUserRepository users, IUnitOfWork unitOfWork)
    : IRequestHandler<SetUserBlockedCommand>
{
    public async Task Handle(SetUserBlockedCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, ct);
            var user = await UserWorkflow.TargetAsync(request.UserId, users, ct);
            if (request.IsBlocked && !user.IsBlocked && user.HasRole(RoleNames.Admin)
                && await users.CountActiveAdminsAsync(ct) <= 1)
                throw new ConflictException("Не можна заблокувати останнього активного адміністратора.");
            if (request.IsBlocked) user.Block(); else user.Unblock();
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
