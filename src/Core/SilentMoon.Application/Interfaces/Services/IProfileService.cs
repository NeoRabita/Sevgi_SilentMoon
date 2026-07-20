using SilentMoon.Application.DTOs.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<UserProfileResponse> GetUserProfileAsync();
        Task<Result<UpdateUserProfileResponse>> UpdateUserProfileAsync(string name);
    }
}
