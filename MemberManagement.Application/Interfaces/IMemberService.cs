using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetActiveMembersAsync();
        Task<Member?> GetByIdAsync(int id);
        Task CreateAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(int id);
    }
}
