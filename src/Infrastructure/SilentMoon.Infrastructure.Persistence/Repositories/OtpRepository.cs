using SilentMoon.Application.Interfaces.Caching;
using SilentMoon.Application.Interfaces.Repositories;
using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SilentMoon.Infrastructure.Persistence.Repositories
{
    public class OtpRepository : IOtpRepository
    {

        private readonly ICacheService _cacheService;

        public OtpRepository(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<OTPCode> GetByEmailAsyncOrCodeAsync(string email, string code)
        {
            var key = $"otp:{email}:{code}";
            return await _cacheService.GetAsync<OTPCode>(key);
        }

        public async Task IncrementAttempsAsync(string email, string code)
        {
            var key = $"otp:{email}:{code}";
            var otp=await _cacheService.GetAsync<OTPCode>(key);
            if (otp != null)
            {
                otp.Attempts++;
                await _cacheService.SetAsync(key, otp);            }
        }

        public async Task MarkAsUsedAsync(string email, string code)
        {
            var key = $"otp:{email}:{code}";
            await _cacheService.RemoveAsync(key);
        }

        public async Task SaveAsync(OTPCode otpCode)
        {
            var key = $"otp:{otpCode.Email}:{otpCode.Code}";
            await _cacheService.SetAsync(key, otpCode, TimeSpan.FromMinutes(10));
        }
    }
}
