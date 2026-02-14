using MemberManagement.Domain.Entities;

namespace MemberManagement.Domain.Interfaces
{
    public interface IMembershipTypeRepository
    {
        Task<MembershipType?> GetByIdAsync(int id);
        Task<IEnumerable<MembershipType>> GetAllAsync();
        Task AddAsync(MembershipType membershipType);
        Task UpdateAsync(MembershipType membershipType);
        Task DeleteAsync(int id);
    }
}