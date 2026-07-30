using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Commands.OTP
{
    public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.OtpId)
                .NotEmpty().WithMessage("OTP Id is required.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(6).WithMessage("OTP code must be 6 digits.")
                .Matches(@"^\d{6}$").WithMessage("OTP code must contain only digits."); 

        }
    }
}
