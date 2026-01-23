using FluentValidation;
using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Validation
{
    // install FluentValidation via NuGet if not already installed
    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("Last Name is required.");
            RuleFor(x => x.BirthDate)
                .NotNull().WithMessage("BirthDate is required.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("BirthDate cannot be in the future.");
        }
    }
}
