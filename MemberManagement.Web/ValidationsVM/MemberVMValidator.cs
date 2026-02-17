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

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(50).WithMessage("Last Name is too long.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("Last Name should only contain letters.");

            RuleFor(x => x)
                .MustAsync(async (x, cancellation) =>
                {
                    var exists = await _context.Members.AnyAsync(m =>
                        m.FirstName.ToLower() == x.FirstName.ToLower() &&
                        m.LastName.ToLower() == x.LastName.ToLower() &&
                        m.BirthDate == DateOnly.FromDateTime(x.BirthDate.Value) &&
                        m.MemberID != x.MemberID, cancellation);
                    return !exists;
                })
                .WithMessage("A member with this name and birthdate already exists.")
                .When(x => !string.IsNullOrEmpty(x.FirstName) && !string.IsNullOrEmpty(x.LastName) && x.BirthDate.HasValue);

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
                .NotEmpty().WithMessage("Contact Number is required.")
                .Matches(@"^09\d{9}$").WithMessage("Format: 09XXXXXXXXX (11 digits)")
                .MustAsync(async (x, contact, cancellation) =>
                {
                    return !await _context.Members.AnyAsync(m => m.ContactNo == contact && m.MemberID != x.MemberID, cancellation);
                }).WithMessage("This contact number is already in use.");

            RuleFor(m => m.EmailAddress)
                .NotEmpty().WithMessage("Email Address is required.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Please enter a valid email.")
                .MustAsync(async (x, email, cancellation) =>
                {
                    return !await _context.Members.AnyAsync(m => m.EmailAddress == email && m.MemberID != x.MemberID, cancellation);
                }).WithMessage("This email address is already registered.");
        }
    }
}