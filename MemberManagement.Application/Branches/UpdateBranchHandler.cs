using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.Branches
{
    public class UpdateBranchHandler
    {
        private readonly IBranchRepository _repository;

        public UpdateBranchHandler(IBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(int id, string name, string address, string branchCode)
        {
            var branch = await _repository.GetByIdAsync(id);

            if (branch == null)
                throw new KeyNotFoundException("Branch not found.");

            branch.Update(name, address, branchCode);

            await _repository.UpdateAsync(branch);
        }
    }
}