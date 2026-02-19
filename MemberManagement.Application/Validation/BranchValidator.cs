using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Application.Validation
{
    public class BranchValidator : AbstractValidator<Branch>
    {
        private readonly MMSDbContext _context;

        public BranchValidator(MMSDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Branch Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.BranchCode)
                .NotEmpty().WithMessage("Branch Code is required.")
                .MustAsync(async (branch, code, cancellation) =>
                {
                    return !await _context.Branches.AnyAsync(b =>
                        b.BranchCode == code && b.Id != branch.Id, cancellation);
                }).WithMessage("Branch code already exists.");
        }
    }
}