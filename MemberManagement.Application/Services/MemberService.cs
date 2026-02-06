using FluentValidation;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IValidator<Member> _validator;

        public MemberService(IMemberRepository repository, IValidator<Member> validator)
        {
            _memberRepository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Member member)
        {
            var result = await _validator.ValidateAsync(member);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            // Using the Domain Method
            member.Initialize();

            await _memberRepository.AddAsync(member);
        }

        public async Task UpdateAsync(Member member)
        {
            var result = await _validator.ValidateAsync(member);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _memberRepository.UpdateAsync(member);
        }

        public Task DeleteAsync(int id)
        {
            return _memberRepository.SoftDeleteAsync(id);
        }
    }
}

/* HOW IT WORKS:
  This service centralizes the business logic for Members, ensuring that no 
  member is created or updated without first passing through your safety checks.

  1. REPOSITORY PATTERN: It uses 'IMemberRepository' to handle the actual database 
     work (SQL, Entity Framework, etc.). This keeps the service focused on 
     business logic rather than database syntax.

  2. DOUBLE-SURE VALIDATION: Both 'CreateAsync' and 'UpdateAsync' call the 
     validator. This ensures that even if a developer forgets to validate in 
     the UI or the Handler, the Service Layer acts as a final safety net 
     before the data hits the database.

  3. DOMAIN INITIALIZATION: In 'CreateAsync', it calls 'member.Initialize()'. 
     This is a "Domain-Driven" approach where the Member entity itself 
     decides its starting state (like setting 'IsActive = true' or 
     'DateCreated = DateTime.Now') rather than the service doing it.

  4. SOFT DELETE: Notice 'DeleteAsync' calls 'SoftDeleteAsync'. This usually 
     means the record isn't actually erased from the database; instead, 
     a flag (like 'IsDeleted' or 'IsActive = false') is flipped so you 
     keep a history of the member for record-keeping.

  5. ABSTRACTION: By implementing 'IMemberService', this class can be swapped 
     out or mocked in tests, making your application very modular and 
     easy to maintain.
*/