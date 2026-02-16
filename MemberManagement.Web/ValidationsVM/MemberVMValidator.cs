using FluentValidation;
using MemberManagement.Web.ViewModels;
using MemberManagement.SharedKernel.Constant;

namespace MemberManagement.Web.ValidationsVM
{
    public class MemberVMValidator : AbstractValidator<MemberVM>
    {
        public MemberVMValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50).WithMessage("First Name is too long.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(50).WithMessage("Last Name is too long.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage(OperationMessage.Error.BirthDateRequired)
                .DependentRules(() => {
                    RuleFor(x => x.BirthDate)
                        .Must(date => date.HasValue && date.Value.Date <= DateTime.Today)
                        .WithMessage("Birth date cannot be in the future.");

                    RuleFor(x => x.BirthDate)
                        .Must(date => date.HasValue && DateOnly.FromDateTime(date.Value) <= DateOnly.FromDateTime(DateTime.Today).AddYears(-18))
                        .WithMessage(OperationMessage.Error.Underage);

                    RuleFor(x => x.BirthDate)
                        .Must(date => date.HasValue && DateOnly.FromDateTime(date.Value) >= DateOnly.FromDateTime(DateTime.Today).AddYears(-65).AddMonths(-6).AddDays(-1))
                        .WithMessage(OperationMessage.Error.ExceedsAgeLimit);
                });

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Please select a branch.")
                .GreaterThan(0).WithMessage("Invalid branch selection.");

            RuleFor(x => x.MembershipTypeId)
                .NotEmpty().WithMessage("Please select a membership type.")
                .GreaterThan(0).WithMessage("Invalid membership type selection.");

            RuleFor(m => m.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Address));

            RuleFor(m => m.ContactNo)
                .Matches(@"^09\d{9}$")
                .WithMessage("Format: 09XXXXXXXXX (11 digits)")
                .When(m => !string.IsNullOrWhiteSpace(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Please enter a valid email.")
                .When(m => !string.IsNullOrWhiteSpace(m.EmailAddress));
        }
    }
}