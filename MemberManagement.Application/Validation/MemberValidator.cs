using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.SharedKernel.Constant;

namespace MemberManagement.Application.Validation
{
    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("Last Name is required.");

            RuleFor(m => m.BirthDate)
                .NotEmpty().WithMessage(OperationMessage.Error.BirthDateRequired)
                .Must(beAtLeast18).WithMessage(OperationMessage.Error.Underage)
                .Must(beUnderLimit).WithMessage(OperationMessage.Error.ExceedsAgeLimit);

            RuleFor(m => m.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Address));

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("Please select a valid branch.");

            RuleFor(m => m.ContactNo)
                .Matches(@"^09\d{9}$").WithMessage("Invalid PH number. Format: 09XXXXXXXXX")
                .When(m => !string.IsNullOrWhiteSpace(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email address.")
                .When(m => !string.IsNullOrEmpty(m.EmailAddress));
        }

        private bool beAtLeast18(DateOnly? date) =>
            date.HasValue && date <= DateOnly.FromDateTime(DateTime.Today).AddYears(-18);

        private bool beUnderLimit(DateOnly? date) =>
            date.HasValue && date >= DateOnly.FromDateTime(DateTime.Today).AddYears(-65).AddMonths(-6).AddDays(-1);
    }
}