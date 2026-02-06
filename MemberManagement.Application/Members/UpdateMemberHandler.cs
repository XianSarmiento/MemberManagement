using FluentValidation;
using MemberManagement.Application.Core;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Application.Members
{
    public class UpdateMemberHandler
    {
        private readonly IMemberService _memberService;
        private readonly IValidator<Member> _validator;

        public UpdateMemberHandler(IMemberService memberService, IValidator<Member> validator)
        {
            _memberService = memberService;
            _validator = validator;
        }

        public async Task HandleAsync(MemberDTO dto)
        {
            var member = await _memberService.GetByIdAsync(dto.MemberID)
                ?? throw new KeyNotFoundException(OperationMessage.Error.NotFound);

            UpdateMemberFields(member, dto);

            var result = await _validator.ValidateAsync(member);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _memberService.UpdateAsync(member);
        }

        private void UpdateMemberFields(Member member, MemberDTO dto)
        {
            member.FirstName = dto.FirstName!;
            member.LastName = dto.LastName!;
            member.BirthDate = dto.BirthDate.HasValue
                ? DateOnly.FromDateTime(dto.BirthDate.Value)
                : null;
            member.Address = dto.Address;
            member.Branch = dto.Branch;
            member.ContactNo = dto.ContactNo;
            member.EmailAddress = dto.EmailAddress;
        }
    }
}

/* HOW IT WORKS:
  This handler manages the lifecycle of updating an existing member's record. 
  It ensures data integrity by following a strict sequence:

  1. RETRIEVAL & GUARD CLAUSE: It first attempts to find the existing record in the 
     database using the ID provided in the DTO. If the record doesn't exist, it 
     immediately throws a 'KeyNotFoundException' to prevent orphaned updates.

  2. MANUAL MAPPING: The 'UpdateMemberFields' helper method copies values from the 
     incoming DTO onto the existing 'Member' entity. This "patching" approach is 
     safer than creating a new object because it preserves the entity's state 
     and tracking history within the database context.

  3. DATE CONVERSION: Inside the mapping logic, it handles the conversion from 
     'DateTime' (standard in DTOs/JSON) to 'DateOnly' (specific to the Domain Entity), 
     demonstrating a clean separation between API types and Domain types.

  4. VALIDATION: Even though the record already existed, the handler re-validates 
     the entity after the updates are applied. This ensures that the new changes 
     don't violate any business rules (e.g., changing an email to an invalid format).

  5. PERSISTENCE: Once validated, it calls 'UpdateAsync'. Because the entity was 
     fetched earlier, the service can efficiently push only the changed values 
     back to the database.
*/