using FluentValidation;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Interfaces;
using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Branches
{
    public class UpdateBranchHandler
    {
        private readonly IBranchRepository _repository;
        private readonly IValidator<Branch> _validator; // Add validator

        public UpdateBranchHandler(IBranchRepository repository, IValidator<Branch> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task Handle(int id, string name, string address, string branchCode)
        {
            var branch = await _repository.GetByIdAsync(id);

            if (branch == null)
                throw new KeyNotFoundException("Branch not found.");

            // 1. Update the entity state
            branch.Update(name, address, branchCode);

            // 2. Validate the updated state (checks for duplicate codes, etc.)
            var result = await _validator.ValidateAsync(branch);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _repository.UpdateAsync(branch);
        }
    }
}