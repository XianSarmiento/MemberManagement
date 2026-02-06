using MemberManagement.Application.DTOs;
using MemberManagement.Domain.Entities;

// Converts Entity : DTO
namespace MemberManagement.Application.Mappers
{
    public static class MemberVMMapper
    {
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
  This static class acts as the "translator" between the Domain layer and the 
  Application layer. It ensures that internal database logic is separated from 
  the data sent to the UI.

  1. DOMAIN VS. DTO: In Clean Architecture, we never expose our database Entities 
     directly. This mapper allows the application to work with 'MemberDTO', 
     protecting the 'Member' Entity from direct external modification.

  2. DATA TYPE CONVERSION (DateOnly vs. DateTime):
     - Domain Entities often use 'DateOnly' for birthdates to avoid time-zone math.
     - DTOs/JSON use 'DateTime' because it is more standard for serialization.
     - ToDto: Converts 'DateOnly' to 'DateTime' by adding a dummy 'TimeOnly.MinValue'.
     - ToEntity: Converts 'DateTime' back to 'DateOnly' using 'FromDateTime'.

  3. SELECTIVE MAPPING (Security):
     - Notice that 'ToDto' includes 'IsActive' and 'DateCreated'. The UI needs to see these.
     - However, 'ToEntity' does NOT map 'DateCreated' or 'IsActive' from the DTO. 
       This prevents a malicious user from trying to change their join date or 
       reactivate their own account by sending those values in a request.

  4. EXTENSION METHODS: By using the 'this' keyword in parameters, you can use 
     clean syntax in your handlers, such as 'var member = dto.ToEntity();'.

  5. NULL HANDLING: It safely handles optional fields (like BirthDate) using 
     null-conditional operators and '.HasValue' checks to prevent crashes.
*/