using FluentValidation;
using MemberManagement.Web.ViewModels;
using MemberManagement.SharedKernel.Constant;

namespace MemberManagement.Web.ValidationsVM
{
    public class MemberVMValidator : AbstractValidator<MemberVM>
    {
        public MemberVMValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage(OperationMessage.Error.BirthDateRequired)
                .Must(date => date.HasValue && date.Value.Date <= DateTime.Today)
                .WithMessage("Birth date cannot be in the future");

            // Check Minimum Age
            RuleFor(x => x.BirthDate)
                .Must(date => date.HasValue && DateOnly.FromDateTime(date.Value) <= DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
                .WithMessage(OperationMessage.Error.Underage)
                .When(x => x.BirthDate.HasValue);

            // Check Maximum Age (Added to match your domain logic)
            RuleFor(x => x.BirthDate)
                .Must(date => date.HasValue && DateOnly.FromDateTime(date.Value) >= DateOnly.FromDateTime(DateTime.Today).AddYears(-65).AddMonths(-6).AddDays(-1))
                .WithMessage(OperationMessage.Error.ExceedsAgeLimit)
                .When(x => x.BirthDate.HasValue);

            RuleFor(m => m.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Address));

            RuleFor(m => m.ContactNo)
                .Matches(@"^09\d{9}$").WithMessage("Invalid PH number.")
                .When(m => !string.IsNullOrWhiteSpace(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email address.")
                .When(m => !string.IsNullOrEmpty(m.EmailAddress));
        }
    }
}