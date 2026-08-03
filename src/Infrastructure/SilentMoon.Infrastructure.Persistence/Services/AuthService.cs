using Google.Apis.Auth;
using SilentMoon.Application.DTOs.Account;
using SilentMoon.Application.DTOs.JWT;
using SilentMoon.Application.Interfaces.Messaging;
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
    public class AuthService : IAuthService
    {
        private readonly IOtpService _otpService;
        private readonly IGenericRepository<ApplicationUser> _genericRepository;


        public AuthService(IOtpService otpService, IJwtService jwtService, IGenericRepository<ApplicationUser> genericRepository)
        {
            _otpService = otpService;
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

            }

            return Result<ApplicationUser>.Success(user);

        }





        //public async Task<Result<RegisterResponse>> ResendOtp(string otpId)
        //{
        //var otp = await _otpService.GetOtpCodeAsync(otpId);
        //    if(otp.IsFailure)
        //    {
        //        return otp.Error;
        //    }

        //    var newOtp = await _otpService.CreateOtpCodeAsync(otp.Value.Email, "Email Verification", "Your verification code is: ");

    //        return Result<RegisterResponse>.Success(new RegisterResponse
    //        {
    //            Message = "Please check your email for verification code.",
    //            OtpId = newOtp.Id.ToString(),
    //            OtpExpireAt = newOtp.ExpiresAt.ToShortDateString()
    //});



        //}





    }
}
