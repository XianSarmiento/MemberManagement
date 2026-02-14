using FluentValidation;
using MemberManagement.Domain.Entities;

// MemberValidator (Domain): Validates the Business Rule. It ensures the actual record is valid before it hits the database.
namespace MemberManagement.Application.Validation
{
    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First Name is required.");

            RuleFor(m => m.LastName)
                .NotEmpty().WithMessage("Last Name is required.");

            RuleFor(m => m.BirthDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("BirthDate cannot be in the future.")
                .When(m => m.BirthDate.HasValue);

            RuleFor(m => m.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Address));

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("Please select a valid branch.");

            RuleFor(m => m.ContactNo)
                .Matches(@"^09\d{9}$").WithMessage("Invalid PH number.")
                .When(m => !string.IsNullOrEmpty(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email address.")
                .When(m => !string.IsNullOrEmpty(m.EmailAddress));
        }
    }
}
