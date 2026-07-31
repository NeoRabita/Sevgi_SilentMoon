using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentMoon.Application.Features.Topics.Commands
{
    public class SetSelectedTopicsCommandValidator : AbstractValidator<SetSelectedTopicsCommand>
    {
        public SetSelectedTopicsCommandValidator()
        {
            RuleFor(x => x.TopicIds)
    .NotNull().WithMessage("At least one topic must be selected.")
    .NotEmpty().WithMessage("At least one topic must be selected.");

            RuleForEach(x => x.TopicIds)
                .Equal(0).WithMessage("Invalid topic id.");
        }
    }
}
