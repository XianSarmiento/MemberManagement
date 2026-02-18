using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.MembershipTypes
{
    public class CreateMembershipTypeHandler
    {
        private readonly IMembershipTypeRepository _repository;

        public CreateMembershipTypeHandler(IMembershipTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(string name, string membershipCode, decimal fee, string description)
        {
            var membershipType = new MembershipType(name, membershipCode, fee, description);

            await _repository.AddAsync(membershipType);
            return membershipType.Id;
        }
    }
}