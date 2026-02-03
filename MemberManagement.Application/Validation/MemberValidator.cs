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
            RuleFor(m => m.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(m => m.Branch).NotEmpty().WithMessage("Branch is required.");
            RuleFor(m => m.ContactNo)
                .NotEmpty().WithMessage("Mobile No. is required.")
                .Matches(@"^09\d{9}$")
                .WithMessage("Invalid PH number.");
            RuleFor(m => m.EmailAddress)
                .NotEmpty().WithMessage("Email Address is required.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Invalid email address..");
        }
    }
}
