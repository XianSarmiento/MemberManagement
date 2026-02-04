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

            // Optional fields (Only validate if provided or if not required)
            RuleFor(m => m.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Address));

            RuleFor(m => m.Branch)
                .MaximumLength(50).WithMessage("Branch cannot exceed 50 characters.")
                .When(m => !string.IsNullOrEmpty(m.Branch));

            RuleFor(m => m.ContactNo)
                .Matches(@"^09\d{9}$").WithMessage("Invalid PH number.")
                .When(m => !string.IsNullOrEmpty(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email address.")
                .When(m => !string.IsNullOrEmpty(m.EmailAddress));
        }
    }
}
