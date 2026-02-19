using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;

namespace MemberManagement.Application.MembershipTypes
{
    public class CreateMembershipTypeHandler
    {
        private readonly IMembershipTypeRepository _repository;
        private readonly IValidator<MembershipType> _validator; // Add validator

        public CreateMembershipTypeHandler(IMembershipTypeRepository repository, IValidator<MembershipType> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<int> Handle(string name, string membershipCode, decimal fee, string description)
        {
            var membershipType = new MembershipType(name, membershipCode, fee, description);

            // Validate
            var result = await _validator.ValidateAsync(membershipType);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _repository.AddAsync(membershipType);
            return membershipType.Id;
        }
    }
}