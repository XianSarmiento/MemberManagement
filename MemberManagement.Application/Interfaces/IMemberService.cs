using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetActiveMembersAsync();
        Task<IEnumerable<Member>> GetInactiveMembersAsync();
        Task RestoreAsync(int id);
        Task<Member?> GetByIdAsync(int id);
        Task CreateAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(int id);
    }
}