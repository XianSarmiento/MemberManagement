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

        public async Task<int> Handle(string name, decimal fee)
        {
            // Enforces the logic defined in your Domain Entity constructor
            var membershipType = new MembershipType(name, fee);

            await _repository.AddAsync(membershipType);
            return membershipType.Id;
        }
    }
}