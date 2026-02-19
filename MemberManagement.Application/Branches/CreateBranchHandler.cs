using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.Branches
{
    public class CreateBranchHandler
    {
        private readonly IBranchRepository _repository;
        private readonly IValidator<Branch> _validator; // Add validator

        public CreateBranchHandler(IBranchRepository repository, IValidator<Branch> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<int> Handle(string branchName, string address, string branchCode)
        {
            var branch = new Branch(branchName, address, branchCode);

            // 1. Validate the new branch
            var result = await _validator.ValidateAsync(branch);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _repository.AddAsync(branch);
            return branch.Id;
        }
    }
}