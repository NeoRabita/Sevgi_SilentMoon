using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.DTOs.Account
{
    public class RegisterResponse
    {
        public string Message { get; set; }
        public string OtpId { get; set; }
        public string OtpExpireAt { get; set; }
    }
}
