using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Application.DTOs.JWT;
using SilentMoon.Domain.Entities;
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
        private readonly IJwtService _jwtService;

        public AuthService(IOtpService otpService, IAuthRepository authRepo, IJwtService jwtService)
        {
            _otpService = otpService;
            _authRepo = authRepo;
            _jwtService = jwtService;
        }

        //public async Task<Result<RegisterResponse>> LoginAsync(AuthenticationRequest request)
        //{
        //    if (!(await _authRepo.UserExistsAsync(request.Email))) {
        //        return UserErrors.NotFoundByEmail;
        //    }
        //    var auser=await _authRepo.GetApplicationUserByEmailAsync(request.Email);


        //}

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

       

        public async Task<Result<AuthenticationResponse>> VerifyEmailAsync(string email, string code)
        {
          var isVerified= await _otpService.VerifyOtpCode(email, code);
            if (!isVerified)
            {
               return OtpErrors.InvalidCode;
            }
            var user = await _authRepo.GetApplicationUserByEmailAsync(email);
            if (user == null)
            {
                return UserErrors.NotFoundByEmail;
            }
            if (!user.IsEmailConfirmed)
            {
                await _authRepo.ActivateApplicationUser(user.Id);
            }

            var jwt= _jwtService.GenerateToken(user.Id,email);


            var refreshToken = new RefreshToken
            {
                Token = _jwtService.GenerateRefreshToken(),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = "127.0.0.1",
                IsRevoked = false
            };
            await _authRepo.SaveRefreshTokenAsync(refreshToken);
            return Result<AuthenticationResponse>.Success(new AuthenticationResponse
            {
                Name = user.FirstName,
                Jwt = jwt,
                Email = email,
                RefreshToken = new RefreshTokenDto(refreshToken.Token, new DateTimeOffset(refreshToken.Expires, TimeSpan.Zero))
            });

        }
    }
}
