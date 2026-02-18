using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.MembershipTypes
{
    public class UpdateMembershipTypeHandler
    {
        private readonly IMembershipTypeRepository _repository;

        public UpdateMembershipTypeHandler(IMembershipTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(int id, string name, string membershipCode, decimal fee, string description)
        {
            var membershipType = await _repository.GetByIdAsync(id);

            if (membershipType == null)
                throw new KeyNotFoundException("Membership type not found.");

            membershipType.Update(name, membershipCode, fee, description);

            await _repository.UpdateAsync(membershipType);
        }
    }
}