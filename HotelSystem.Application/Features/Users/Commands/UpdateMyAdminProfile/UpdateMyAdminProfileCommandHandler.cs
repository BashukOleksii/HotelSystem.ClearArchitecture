using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyAdminProfile;

public sealed class UpdateMyAdminProfileCommandHandler(IUserRepository users, IUserProfileRepository profiles, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMyAdminProfileCommand>
{
    public async Task Handle(UpdateMyAdminProfileCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var user = await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Admin, users, ct);
            var profile = await profiles.GetAdminAsync(user.Id, ct);
            if (profile is null)
            {
                profile = AdminProfile.Create(user.Id, request.EmployeeNumber, request.DepartmentName);
                profiles.Add(profile);
            }
            else profile.UpdateDetails(request.EmployeeNumber, request.DepartmentName);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
