using Application.Abstractions.Messaging;
using SilentMoon.Application.DTOs.Profile;
using SilentMoon.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Queries.Profile
{
    public class GetUserProfileQuery:IQuery<UserProfileResponse>
    {
    }

    public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserProfileResponse>
    {
        private readonly IProfileService _profileService;

        public GetUserProfileQueryHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery query, CancellationToken ct)
        {
            return await _profileService.GetUserProfileAsync();
        }
    }

}
