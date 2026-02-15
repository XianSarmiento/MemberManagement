using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.Services
{
    public class MembershipTypeService : IMembershipTypeService
    {
        private readonly IMembershipTypeRepository _repository;

        public MembershipTypeService(IMembershipTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MembershipType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}