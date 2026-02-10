using MemberManagement.Application.DTOs;
using MemberManagement.Domain.Entities;

// Converts Entity → DTO
namespace MemberManagement.Application.Mappers
{
    public static class MemberEntityMapper
    {
        // Converts Member (Entity) → MemberDTO
        public static MemberDTO ToDto(this Member member)
        {
            return new MemberDTO
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate?.ToDateTime(TimeOnly.MinValue),
                Address = member.Address,
                Branch = member.Branch,
                ContactNo = member.ContactNo,
                EmailAddress = member.EmailAddress,
                IsActive = member.IsActive,
                DateCreated = member.DateCreated
            };
        }

        // Converts MemberDTO → Member (Entity)
        public static Member ToEntity(this MemberDTO dto)
        {
            return new Member
            {
                MemberID = dto.MemberID,
                FirstName = dto.FirstName!,
                LastName = dto.LastName!,
                BirthDate = dto.BirthDate.HasValue
                    ? DateOnly.FromDateTime(dto.BirthDate.Value)
                    : null,
                Address = dto.Address,
                Branch = dto.Branch,
                ContactNo = dto.ContactNo,
                EmailAddress = dto.EmailAddress
            };
        }
    }
}

/* HOW IT WORKS:
  This static class provides extension methods that allow for easy conversion between 
  Domain Entities and Data Transfer Objects (DTOs).

  1. THE EXTENSION PATTERN: By using the 'this' keyword in the parameters (e.g., 'this Member member'), 
     you can call these methods directly on the object, like 'myMember.ToDto()'. This 
     makes the code in your handlers very clean and readable.

  2. DATA ENCAPSULATION & SECURITY:
     - ToDto: Translates a database entity into a DTO. Note that it includes fields like 
       'IsActive' and 'DateCreated' which are useful for displaying in the UI.
     - ToEntity: Translates a DTO back into a database entity. Notice it does NOT map 
       'DateCreated' or 'IsActive'. This is a security measure to prevent a user from 
       manually changing their account's creation date or status via a request.

  3. TYPE ADAPTATION (DateOnly vs DateTime):
     - The database/domain often uses 'DateOnly' for birthdays to avoid timezone issues.
     - The UI/JSON usually uses 'DateTime' because it is more standard for serializing.
     - This mapper handles that conversion logic in one central place so you don't 
       have to repeat 'ToDateTime' or 'FromDateTime' logic everywhere in your app.

  4. NULL SAFETY: It uses null-conditional operators ('?.') and checks ('.HasValue') 
     to ensure that if a date is missing, the application doesn't crash.
*/