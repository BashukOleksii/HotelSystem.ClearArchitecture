using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Application.Common.Interfaces;
using HotelSystem.Application.Features.Users.Common;
using HotelSystem.Application.Features.Users.DTOs;
using HotelSystem.Domain.Constants;
using HotelSystem.Domain.Entities;
using HotelSystem.Domain.Interfaces;
using MediatR;

namespace HotelSystem.Application.Features.Users.Commands.UpdateMyOwnerProfile;

public sealed class UpdateMyOwnerProfileCommandHandler(IUserRepository users, IUserProfileRepository profiles, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMyOwnerProfileCommand>
{
    public async Task Handle(UpdateMyOwnerProfileCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var user = await UserWorkflow.RequireAsync(request.ActorId, RoleNames.Owner, users, ct);
            var profile = await profiles.GetOwnerAsync(user.Id, ct);
            if (profile is null)
            {
                profile = OwnerProfile.Create(user.Id, request.CompanyName, request.BusinessPhone);
                profiles.Add(profile);
            }
            else profile.UpdateDetails(request.CompanyName, request.BusinessPhone);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);
    }
}
