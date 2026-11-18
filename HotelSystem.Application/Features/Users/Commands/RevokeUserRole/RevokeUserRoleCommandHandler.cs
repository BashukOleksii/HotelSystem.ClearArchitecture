using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.RevokeUserRole;

public sealed class RevokeUserRoleCommandHandler(IUserRepository users, IRoleRepository roles, IUnitOfWork unitOfWork)
    : IRequestHandler<RevokeUserRoleCommand>
{
    public async Task Handle(RevokeUserRoleCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, ct);
            var user = await UserWorkflow.TargetAsync(request.UserId, users, ct);
            var role = await roles.GetByNameAsync(request.RoleName.Trim(), ct)
                ?? throw new NotFoundException("Роль не знайдено.");
            if (role.Name == RoleNames.User)
                throw new ConflictException("Базову роль User відкликати не можна.");
            if (role.Name == RoleNames.Admin && user.HasRole(RoleNames.Admin) && !user.IsBlocked
                && await users.CountActiveAdminsAsync(ct) <= 1)
                throw new ConflictException("Не можна відкликати роль останнього активного адміністратора.");
            user.RevokeRole(role.Id);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
