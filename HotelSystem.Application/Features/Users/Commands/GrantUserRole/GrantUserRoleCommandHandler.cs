using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.GrantUserRole;

public sealed class GrantUserRoleCommandHandler(IUserRepository users, IRoleRepository roles, IUserProfileRepository profiles, IUnitOfWork unitOfWork)
    : IRequestHandler<GrantUserRoleCommand>
{
    public async Task Handle(GrantUserRoleCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, ct);
            var user = await UserWorkflow.TargetAsync(request.UserId, users, ct);
            var role = await roles.GetByNameAsync(request.RoleName.Trim(), ct)
                ?? throw new NotFoundException("Роль не знайдено.");
            if (role.Name == RoleNames.Owner && await profiles.GetOwnerAsync(user.Id, ct) is null)
                profiles.Add(OwnerProfile.Create(user.Id));
            if (role.Name == RoleNames.Admin && await profiles.GetAdminAsync(user.Id, ct) is null)
                profiles.Add(AdminProfile.Create(user.Id));
            user.GrantRole(role);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
