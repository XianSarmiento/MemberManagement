using MemberManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Domain.Interfaces
{
    public interface IMemberRepository
    {
        // Member List Page
        Task<IEnumerable<Member>> GetAllAsync();

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
