using SilentMoon.Application.DTOs.Profile;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using SilentMoon.Domain.Errors;
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
        private readonly IGenericRepository<ApplicationUser> _genericRepository;

        public ProfileService(IUserService userService, IGenericRepository<ApplicationUser> genericRepository)
        {
            _userService = userService;
            _genericRepository = genericRepository;
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

        public async Task<Result<UpdateUserProfileResponse>> UpdateUserProfileAsync(string name)
        {
            var userResult = await _userService.GetCurrentUserAsync();

            if (userResult.IsFailure)
            {
                return userResult.Error;
            }
            var user = userResult.Value;
            user.FirstName = name;
            _genericRepository.Update(user);
            return Result<UpdateUserProfileResponse>.Success(
            new UpdateUserProfileResponse
             {
                 Name = user.FirstName
             });


        }
    }
}
