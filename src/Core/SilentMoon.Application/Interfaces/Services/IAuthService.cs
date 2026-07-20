using SilentMoon.Application.DTOs.Account;
using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<ApplicationUser>> VerifyEmailAsync(string otpId, string code);
        Task<Result<RegisterResponse>> ResendOtp(string otpId);
        //Task<Result<bool>> LoginAsync(AuthenticationRequest request);
        Task<Result<ApplicationUser>> GoogleLoginAsync(string idToken);



    }
}
