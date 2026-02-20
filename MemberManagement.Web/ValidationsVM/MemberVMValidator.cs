using FluentValidation;
using MemberManagement.Web.ViewModels;
using MemberManagement.SharedKernel.Constant;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Web.ValidationsVM
{
    public class MemberVMValidator : AbstractValidator<MemberVM>
    {
        private readonly MMSDbContext _context;

        public MemberVMValidator(MMSDbContext context)
        {
            _context = context;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50).WithMessage("First Name is too long.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("First Name should only contain letters.");

            // 1. Basic Validation for Last Name (Always active)
            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(50).WithMessage("Last Name is too long.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Last Name should only contain letters.");

            // 2. Separate Duplicate Check for Last Name (Conditional)
            RuleFor(x => x.LastName)
                .MustAsync(async (memberVM, lastName, cancellation) =>
                {
                    if (string.IsNullOrEmpty(memberVM.FirstName) || string.IsNullOrEmpty(lastName) || !memberVM.BirthDate.HasValue)
                        return true;

                    var exists = await _context.Members.AnyAsync(m =>
                        m.FirstName.ToLower() == memberVM.FirstName.ToLower() &&
                        m.LastName.ToLower() == lastName.ToLower() &&
                        m.BirthDate == DateOnly.FromDateTime(memberVM.BirthDate.Value) &&
                        m.MemberID != memberVM.MemberID, cancellation);
                    return !exists;
                })
                .WithMessage("A member with this name and birthdate already exists.")
                .When(x => !string.IsNullOrEmpty(x.LastName));

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
                    .When(m => !string.IsNullOrEmpty(m.ContactNo))
                .MustAsync(async (x, contact, cancellation) =>
                {
                    if (string.IsNullOrEmpty(contact)) return true;
                    return !await _context.Members.AnyAsync(m => m.ContactNo == contact && m.MemberID != x.MemberID, cancellation);
                })
                .WithMessage("This contact number is already in use.")
                .When(m => !string.IsNullOrEmpty(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                    .WithMessage("Please enter a valid email.")
                    .When(m => !string.IsNullOrEmpty(m.EmailAddress))
                .MustAsync(async (x, email, cancellation) =>
                {
                    if (string.IsNullOrEmpty(email)) return true;
                    return !await _context.Members.AnyAsync(m => m.EmailAddress == email && m.MemberID != x.MemberID, cancellation);
                })
                .WithMessage("This email address is already registered.")
                .When(m => !string.IsNullOrEmpty(m.EmailAddress));
        }
    }
}