using MemberManagement.Domain.Entities;

namespace MemberManagement.Domain.Interfaces
{
    public interface IBranchRepository
    {
        Task<Branch?> GetByIdAsync(int id);

        Task<IEnumerable<Branch>> GetAllAsync();

        Task AddAsync(Branch branch);

        Task UpdateAsync(Branch branch);

        Task DeleteAsync(int id);
    }
}
