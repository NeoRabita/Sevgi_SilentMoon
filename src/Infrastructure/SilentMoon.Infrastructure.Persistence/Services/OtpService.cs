using Microsoft.Extensions.Options;
using SilentMoon.Application.DTOs.Email;
using SilentMoon.Application.Interfaces.Caching;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
using SilentMoon.Domain.Errors;
using SilentMoon.Infrastructure.Persistence.Settings;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly ICacheService _cacheService;
        private MailSettings _apiSettings;

        public OtpService(IEmailService emailService, ICacheService cacheService, IOptions<MailSettings> apiSettings)
        {
            _emailService = emailService;
            _cacheService = cacheService;
            _apiSettings = apiSettings.Value;
        }

        public async Task<OTPCode> CreateAndSendOtpCodeAsync(string email, string subject, string body)
        {
            var code = GenerateOtpCode();

            var otp = new OTPCode
            {
                Id = Guid.NewGuid(),
                Code = code,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_apiSettings.OtpExpireTime),
                Attempts = 0
            };

            var key = $"otp:{otp.Id}";
            await _cacheService.SetAsync(key, otp, TimeSpan.FromMinutes(_apiSettings.OtpExpireTime));

            await _emailService.SendAsync(new EmailRequest
            {
                To = email,
                Subject = subject,
                Body = $"{body}: {code}"
            });

            return otp;

        }

        public async Task<Result<OTPCode>> GetOtpCodeAsync(string otpId)
        {

            var key = $"otp:{otpId}";
            var otp = await _cacheService.GetAsync<OTPCode>(key);

            if (otp is null)
                return OtpErrors.InvalidCode;

            
            await _cacheService.RemoveAsync(key);

            return Result<OTPCode>.Success(otp);

        }
        public async Task<Result<OTPCode>> VerifyOtpCodeAsync(string otpId,string code)
        {
            var key = $"otp:{otpId}";
            var otp = await _cacheService.GetAsync<OTPCode>(key);

            if (otp is null)
                return OtpErrors.InvalidCode;

            if(otp.Code != code)
            {
                otp.Attempts++;
                await _cacheService.SetAsync(key, otp, TimeSpan.FromMinutes(_apiSettings.OtpExpireTime));
                return OtpErrors.InvalidCode;
            }

            if (otp.IsExpired)
                return OtpErrors.Expired;
            if (!otp.CanAttempt)
                return OtpErrors.AlreadyUsed;

            await _cacheService.RemoveAsync(key);

            return Result<bool>.Success(otp);
        }

        private string GenerateOtpCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
