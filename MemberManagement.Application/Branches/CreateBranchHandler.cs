using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.Branches
{
    public class CreateBranchHandler
    {
        private readonly IBranchRepository _repository;

        public CreateBranchHandler(IBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(string branchName, string address, string branchCode)
        {
            var branch = new Branch(branchName, address, branchCode);

            await _repository.AddAsync(branch);
            return branch.Id;
        }
    }
}