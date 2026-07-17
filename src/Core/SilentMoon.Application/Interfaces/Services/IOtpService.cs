using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Services
{
    public interface IOtpService
    {
        Task<OTPCode> CreateAndSendOtpCodeAsync(string email,string subject,string body);
        Task<Result<OTPCode>> VerifyOtpCodeAsync(string otpId, string code);
        Task<Result<OTPCode>> GetOtpCodeAsync(string otpId);
    }
}
