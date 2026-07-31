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
    {private readonly IUserService _userService;

        public GetUserProfileQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery query, CancellationToken ct)
        {
            var user = await _userService.GetCurrentUserAsync();
            return new UserProfileResponse
            {
                Email = user.Email,
                Id = user.Id,
                CreatedAt = DateTime.Now,
                EmailVerified = user.IsEmailConfirmed,
                Name = user.FirstName,
            };
        }
    }

}
