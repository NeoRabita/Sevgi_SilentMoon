using SilentMoon.Application.DTOs.Email;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Domain.Entities;
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
        private readonly IOtpRepository _otpRepo;
        private readonly IEmailService _emailService;

        public OtpService(IOtpRepository otpRepo, IEmailService emailService)
        {
            _otpRepo = otpRepo;
            _emailService = emailService;
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
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                Attempts = 0
            };

            await _otpRepo.SaveAsync(otp);

            await _emailService.SendAsync(new EmailRequest
            {
                To = email,
                Subject = subject,
                Body = $"{body}: {code}"
            });

            return otp;

        }

        public async Task<bool> VerifyOtpCode(string email, string code)
        {

            var otp = await _otpRepo.GetByEmailAsyncOrCodeAsync(email, code);

            if (otp == null) throw new Exception("Invalid OTP");
            if (otp.IsExpired) throw new Exception("OTP code expired");
            if(!otp.CanAttempt)throw new Exception("Maximum attempts reached");

            await _otpRepo.MarkAsUsedAsync(email, code);
            return true;

        }

        private string GenerateOtpCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
