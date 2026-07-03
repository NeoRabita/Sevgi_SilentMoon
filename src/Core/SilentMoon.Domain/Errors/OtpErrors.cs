using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Domain.Errors
{
        public static class OtpErrors
        {
            public static readonly Error InvalidCode = Error.Validation(
                "Otp.InvalidCode",
                "Invalid or expired OTP code");

            public static readonly Error AlreadyUsed = Error.Validation(
                "Otp.AlreadyUsed",
                "This OTP code has already been used");

            public static readonly Error Expired = Error.Validation(
                "Otp.Expired",
                "OTP code has expired. Please request a new one");

            public static readonly Error TooManyAttempts = Error.Validation(
                "Otp.TooManyAttempts",
                "Too many failed attempts. Please request a new OTP");

            public static readonly Error NotFound = Error.NotFound(
                "Otp.NotFound",
                "OTP code not found");

            public static readonly Error SendFailed = Error.Failure(
                "Otp.SendFailed",
                "Failed to send OTP code");
        }
}
