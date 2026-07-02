using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOtpService _otpService;
        private readonly IAuthRepository _authRepo;

        public AuthService(IOtpService otpService, IAuthRepository authRepo)
        {
            _otpService = otpService;
            _authRepo = authRepo;
        }

        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (await _authRepo.UserExistsAsync(request.Email))
                {

                    return UserErrors.EmailNotUnique;
                }

                var user = await _authRepo.CreateApplicationUserAsync(request);

                var otp = await _otpService.CreateAndSendOtpCodeAsync(user.Email, "Email Verification", "Your verification code is: ");

                return Result<RegisterResponse>.Success(new RegisterResponse
                {
                    Message = "Please check your email for verification code.",
                    Email = user.Email,
                    OtpExpireAt = otp.ExpiresAt.ToShortDateString()

                });
            }
            catch (Exception ex)
            {
                return new Error(
                    "Registration.Failed",
                    $"Registration failed: {ex.Message}",
                    ErrorType.Failure
                );
            }
        }

       

        public Task VerifyEmailAsync(string email, string code)
        {
            throw new NotImplementedException();
        }
    }
}
