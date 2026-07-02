using SilentMoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Interfaces.Repositories
{
    public interface IOtpRepository
    {
        Task SaveAsync(OTPCode otpCode);
        Task<OTPCode> GetByEmailAsyncOrCodeAsync(string email,string code);
        Task MarkAsUsedAsync(string email, string code);
        Task IncrementAttempsAsync(string email, string code);


    }
}
