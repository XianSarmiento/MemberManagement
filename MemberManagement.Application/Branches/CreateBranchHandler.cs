using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using MemberManagement.Application.DTOs;

namespace MemberManagement.Application.Branches
{
    public class CreateBranchHandler
    {
        private readonly IBranchRepository _repository;

        public CreateBranchHandler(IBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(string branchName)
        {
            // Use the Constructor from Step A
            var branch = new Branch(branchName);

            await _repository.AddAsync(branch);
            return branch.Id;
        }
    }
}