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
                .NotEmpty().WithMessage("Membership Name is required.")
                .MaximumLength(100)
                .MustAsync(async (type, name, cancellation) =>
                {
                    var normalizedName = name.ToLower();

                    return !await _context.MembershipTypes.AnyAsync(m =>
                        m.Name.ToLower() == normalizedName &&
                        m.Id != type.Id,
                        cancellation);
                })
                .WithMessage("Membership name already exists.");

            RuleFor(x => x.MembershipFee)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Membership fee cannot be negative.");

            RuleFor(x => x.MembershipCode)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(20)
                .MustAsync(async (type, code, cancellation) =>
                {
                    return !await _context.MembershipTypes.AnyAsync(m =>
                        m.MembershipCode == code && m.Id != type.Id, cancellation);
                })
                .WithMessage("Membership code already exists.");
        }
    }
}