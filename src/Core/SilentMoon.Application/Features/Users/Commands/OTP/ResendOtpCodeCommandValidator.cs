using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Commands.OTP
{
    public class ResendOtpCodeCommandValidator : AbstractValidator<ResendOtpCodeCommand>
    {
        public ResendOtpCodeCommandValidator()
        {
            RuleFor(x => x.OtpId)
               .NotEmpty().WithMessage("OTP Id is required.")
               .NotNull().WithMessage("OTP Id is required.");
        }
    }
}
