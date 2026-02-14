using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Interfaces
{
    public interface IMembershipTypeService
    {
        Task<IEnumerable<MembershipType>> GetAllAsync();
    }
}