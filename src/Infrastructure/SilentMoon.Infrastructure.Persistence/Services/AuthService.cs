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

        public async Task<Result<AuthenticationResponse>> RefreshTokenAsync(string refreshToken)
        {
            var token=await _genericTokenRepository.GetAsync(r=>r.Token==refreshToken);
            if (token is null) { return UserErrors.Unauthorized(); }

            if (!token.IsActive)
            {
                return UserErrors.Unauthorized();
            }

            var user = await _genericRepository.GetAsync(
             x => x.Id == token.UserId);

            if (user is null)
                return UserErrors.NotFoundByEmail;
            var jwt = _jwtService.GenerateToken(user.Id, user.Email);

            token.Token = _jwtService.GenerateRefreshToken();
            token.Created = DateTime.UtcNow;
            token.Expires = DateTime.UtcNow.AddDays(7);
            token.IsRevoked = false;

            _genericTokenRepository.Update(token);

            return Result<AuthenticationResponse>.Success(
                new AuthenticationResponse
                {
                    Name = user.FirstName,
                    Email = user.Email,
                    Jwt = jwt,
                    RefreshToken = new RefreshTokenDto(
                        token.Token,
                        new DateTimeOffset(token.Expires, TimeSpan.Zero))
                });

        }

        public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _genericRepository.GetAsync(
                    x => x.Email == request.Email);

                if (existingUser is not null)
                {
                    return UserErrors.EmailNotUnique;
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = request.Name,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    IsEmailConfirmed = false,
                };

                await _genericRepository.AddAsync(user);

                var otp = await _otpService.CreateAndSendOtpCodeAsync(user.Email, "Email Verification", "Your verification code is: ");

                return Result<RegisterResponse>.Success(new RegisterResponse
                {
                    Message = "Please check your email for verification code.",
                    OtpId = otp.Id.ToString(),
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
