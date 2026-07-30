using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Commands.ForgotPassword
{
    public class ResetPasswordCommandValidation:AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidation() {
            RuleFor(x => x.OtpId)
    .NotEmpty().WithMessage("OTP Id is required.")
    .Must(id => Guid.TryParse(id, out _)).WithMessage("Invalid OTP Id.");

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(6).WithMessage("OTP code must be 6 digits.")
                .Matches(@"^\d{6}$").WithMessage("OTP code must contain only digits.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
        }
    }
}
