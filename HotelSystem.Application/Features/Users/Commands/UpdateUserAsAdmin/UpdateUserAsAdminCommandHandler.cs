using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateUserAsAdmin;

public sealed class UpdateUserAsAdminCommandHandler(IUserRepository users, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateUserAsAdminCommand>
{
    public async Task Handle(UpdateUserAsAdminCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, ct);
            var user = await UserWorkflow.TargetAsync(request.UserId, users, ct);
            await UserWorkflow.EnsureUniqueAsync(request.Details, user.Id, users, ct);
            user.UpdateProfile(request.Details.Username, request.Details.Email, request.Details.PhoneNumber);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
