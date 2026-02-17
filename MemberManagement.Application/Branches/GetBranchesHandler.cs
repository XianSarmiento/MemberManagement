using MemberManagement.Application.DTOs;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.Branches
{
    public class GetBranchesHandler
    {
        private readonly IBranchRepository _repository;

        public GetBranchesHandler(IBranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BranchDto>> Handle()
        {
            var branches = await _repository.GetAllAsync();
            return branches.Select(b => new BranchDto
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                BranchCode = b.BranchCode,
                IsActive = b.IsActive
            });
        }
    }
}