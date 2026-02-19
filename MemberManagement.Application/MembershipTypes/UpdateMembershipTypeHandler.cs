using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.MembershipTypes
{
    public class UpdateMembershipTypeHandler
    {
        private readonly IMembershipTypeRepository _repository;
        private readonly IValidator<MembershipType> _validator; // Add validator

        public UpdateMembershipTypeHandler(IMembershipTypeRepository repository, IValidator<MembershipType> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task Handle(int id, string name, string membershipCode, decimal fee, string description)
        {
            var membershipType = await _repository.GetByIdAsync(id);

            if (membershipType == null)
                throw new KeyNotFoundException("Membership type not found.");

            // Update the entity state
            membershipType.Update(name, membershipCode, fee, description);

            // Validate after update
            var result = await _validator.ValidateAsync(membershipType);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _repository.UpdateAsync(membershipType);
        }
    }
}