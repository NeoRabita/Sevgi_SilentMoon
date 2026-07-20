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
using Google.Apis.Auth;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOtpService _otpService;
        private readonly IJwtService _jwtService;
        private readonly IGenericRepository<ApplicationUser> _genericRepository;
        private readonly IGenericRepository<RefreshToken> _genericTokenRepository;

        public AuthService(IOtpService otpService, IJwtService jwtService, IGenericRepository<RefreshToken> genericTokenRepository, IGenericRepository<ApplicationUser> genericRepository)
        {
            _otpService = otpService;
            _jwtService = jwtService;
            _genericTokenRepository = genericTokenRepository;
            _genericRepository = genericRepository;
        }

        public async Task<Result<ApplicationUser>> GoogleLoginAsync(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var user = await _genericRepository.GetAsync(
                x => x.Email == payload.Email);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    FirstName = payload.Name,
                    IsEmailConfirmed = true,
                    
                    
                };

                await _genericRepository.AddAsync(user);
            }

            return Result<ApplicationUser>.Success(user);

        }

       

      

        public async Task<Result<RegisterResponse>> ResendOtp(string otpId)
        {
            var otp=await _otpService.GetOtpCodeAsync(otpId);
            if(otp.IsFailure)
            {
                return otp.Error;
            }

            var newOtp = await _otpService.CreateAndSendOtpCodeAsync(otp.Value.Email, "Email Verification", "Your verification code is: ");

            return Result<RegisterResponse>.Success(new RegisterResponse
            {
                Message = "Please check your email for verification code.",
                OtpId = newOtp.Id.ToString(),
                OtpExpireAt = newOtp.ExpiresAt.ToShortDateString()
            });



        }

        public async Task<Result<ApplicationUser>> VerifyEmailAsync(string otpId, string code)
        {
            var otp = await _otpService.VerifyOtpCodeAsync(otpId,code);
            if (otp.IsFailure)
            {
                return OtpErrors.InvalidCode;
            }

            var user = await _genericRepository.GetAsync(
                    x => x.Email == otp.Value.Email);
            if (user == null)
            {
                return UserErrors.NotFoundByEmail;
            }
            if (!user.IsEmailConfirmed)
            {
                user.IsEmailConfirmed = true;
                _genericRepository.Update(user);
            }

            return user;


        }



    }
}
