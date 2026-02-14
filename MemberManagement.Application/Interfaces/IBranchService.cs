using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<Branch>> GetAllAsync();
    }
}