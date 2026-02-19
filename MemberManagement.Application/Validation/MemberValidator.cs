using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.SharedKernel.Constant;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Application.Validation
{
    public class MemberValidator : AbstractValidator<Member>
    {
        private readonly MMSDbContext _context;

        public MemberValidator(MMSDbContext context)
        {
            _context = context;

            RuleFor(m => m.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(50);

            RuleFor(m => m.LastName)
                .MustAsync(async (member, lastName, cancellation) =>
                {
                    return !await _context.Members.AnyAsync(m =>
                        m.FirstName.ToLower() == member.FirstName.ToLower() &&
                        m.LastName.ToLower() == member.LastName.ToLower() &&
                        m.BirthDate == member.BirthDate &&
                        m.MemberID != member.MemberID, cancellation);
                })
                .WithMessage("A member with this name and birthdate already exists.")
                .When(m => !string.IsNullOrEmpty(m.FirstName) && !string.IsNullOrEmpty(m.LastName) && m.BirthDate != default);

            RuleFor(m => m.BirthDate)
                .NotEmpty().WithMessage(OperationMessage.Error.BirthDateRequired)
                .DependentRules(() => {
                    RuleFor(m => m.BirthDate)
                        .Must(BeAtLeast18).WithMessage(OperationMessage.Error.Underage)
                        .Must(BeUnderLimit).WithMessage(OperationMessage.Error.ExceedsAgeLimit);
                });

            RuleFor(m => m.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Address));

            RuleFor(x => x.BranchId)
                .GreaterThan(0).WithMessage("Please select a valid branch.");

            RuleFor(x => x.MembershipTypeId)
                .GreaterThan(0).WithMessage("Please select a valid membership type.");

            RuleFor(m => m.ContactNo)
                .Matches(@"^09\d{9}$")
                    .WithMessage("Invalid PH number. Format: 09XXXXXXXXX")
                    .When(m => !string.IsNullOrEmpty(m.ContactNo))
                .MustAsync(async (member, contact, cancellation) =>
                {
                    if (string.IsNullOrEmpty(contact)) return true; // Allow null/empty
                    return !await _context.Members.AnyAsync(m => m.ContactNo == contact && m.MemberID != member.MemberID, cancellation);
                })
                .WithMessage("Contact number already exists.")
                .When(m => !string.IsNullOrEmpty(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                    .WithMessage("Invalid email address.")
                    .When(m => !string.IsNullOrEmpty(m.EmailAddress))
                .MustAsync(async (member, email, cancellation) =>
                {
                    if (string.IsNullOrEmpty(email)) return true; // Allow null/empty
                    return !await _context.Members.AnyAsync(m => m.EmailAddress == email && m.MemberID != member.MemberID, cancellation);
                })
                .WithMessage("Email address already exists.")
                .When(m => !string.IsNullOrEmpty(m.EmailAddress));
        }

        private bool BeAtLeast18(DateOnly? date)
        {
            if (!date.HasValue) return false;
            var today = DateOnly.FromDateTime(DateTime.Today);
            return date <= today.AddYears(-18);
        }

        private bool BeUnderLimit(DateOnly? date)
        {
            if (!date.HasValue) return false;
            var today = DateOnly.FromDateTime(DateTime.Today);
            return date >= today.AddYears(-65).AddMonths(-6).AddDays(-1);
        }
    }
}