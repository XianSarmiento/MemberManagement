using MemberManagement.Application.DTOs;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.MembershipTypes
{
    public class GetMembershipTypesHandler
    {
        private readonly IMembershipTypeRepository _repository;

        public GetMembershipTypesHandler(IMembershipTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MembershipTypeDto>> Handle()
        {
            var types = await _repository.GetAllAsync();
            return types.Select(t => new MembershipTypeDto
            {
                Id = t.Id,
                Name = t.Name,
                MembershipCode = t.MembershipCode,
                Description = t.Description,
                MembershipFee = t.MembershipFee,
                IsActive = t.IsActive
            });
        }
    }
}