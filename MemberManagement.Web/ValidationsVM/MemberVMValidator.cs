using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.Web.ViewModels;

namespace MemberManagement.Web.ValidationsVM
{
    // install FluentValidation via NuGet if not already installed
    public class MemberVMValidator : AbstractValidator<MemberVM>
    {
        public MemberVMValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
            RuleFor(x => x.BirthDate)
                .NotNull().WithMessage("BirthDate is required.")
                .Must(date => date.Date <= DateTime.Today)
                .WithMessage("Birth date cannot be in the future");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.Branch).NotEmpty().WithMessage("Branch is required.");
            RuleFor(m => m.ContactNo)
                .NotEmpty().WithMessage("Mobile No. is required.")
                .Matches(@"^09\d{9}$")
                .WithMessage("Invalid PH number.");
            RuleFor(m => m.EmailAddress)
                .NotEmpty().WithMessage("Email Address is required.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Invalid email address.");
        }
    }
}