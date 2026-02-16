using MemberManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Domain.Interfaces
{
    public interface IMemberRepository
    {
        // Modified to allow fetching inactive records
        Task<IEnumerable<Member>> GetAllAsync(bool onlyActive = true);

        // Member List Page (only active members)
        Task<IEnumerable<Member>> GetActiveAsync();

        // Get Member ID
        Task<Member?> GetByIdAsync(int memberId);

        // Create Member
        Task AddAsync(Member member);

        // Edit/Update Member
        Task UpdateAsync(Member member);

        // Soft delete (set IsActive = false)
        Task SoftDeleteAsync(int memberId);
    }
}
