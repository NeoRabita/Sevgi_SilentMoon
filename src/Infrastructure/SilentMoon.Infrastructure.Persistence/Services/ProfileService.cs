using SilentMoon.Application.DTOs.Profile;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userService;

        public ProfileService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserProfileResponse> GetUserProfileAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            return new UserProfileResponse
            {
                Email = user.Value.Email,
                Id = user.Value.Id,
                CreatedAt = DateTime.Now,
                EmailVerified = user.Value.IsEmailConfirmed,
                Name = user.Value.FirstName,
            };
        }
    }
}
