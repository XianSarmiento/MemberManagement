using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Application.Validation
{
    public class MembershipTypeValidator : AbstractValidator<MembershipType>
    {
        private readonly MMSDbContext _context;

        public MembershipTypeValidator(MMSDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Membership Name is required.");

            RuleFor(x => x.MembershipFee)
                .GreaterThanOrEqualTo(0).WithMessage("Membership fee cannot be negative.");

            RuleFor(x => x.MembershipCode)
                .NotEmpty().WithMessage("Code is required.")
                .MustAsync(async (type, code, cancellation) =>
                {
                    return !await _context.MembershipTypes.AnyAsync(m =>
                        m.MembershipCode == code && m.Id != type.Id, cancellation);
                }).WithMessage("Membership code already exists.");
        }
    }
}