using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Users.Commands.Profile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.Name)
    .NotEmpty().WithMessage("Name is required.")
    .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
    .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
        }
    }
}
