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
                .MaximumLength(20);

            RuleFor(x => x)
                .MustAsync(async (branch, cancellation) =>
                {
                    return !await _context.Branches.AnyAsync(b =>
                        b.Name == branch.Name &&
                        b.Address == branch.Address &&
                        b.BranchCode == branch.BranchCode &&
                        b.Id != branch.Id,
                        cancellation);
                })
                .WithMessage("Branch already exists with the same name, address, and code.");
        }
    }
}