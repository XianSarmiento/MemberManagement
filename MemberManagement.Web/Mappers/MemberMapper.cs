using MemberManagement.Application.DTOs;
using MemberManagement.Web.ViewModels;

// Converts MemberDTO → MemberVM
namespace MemberManagement.Web.Mappers
{
    public static class MemberMapper
    {
        // Converts MemberDTO → MemberVM
        public static MemberVM ToViewModel(this MemberDTO dto)
        {
            return new MemberVM
            {
                MemberID = dto.MemberID,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Branch = dto.Branch,
                ContactNo = dto.ContactNo,
                EmailAddress = dto.EmailAddress
            };
        }

        // Converts MemberVM → MemberDTO
        public static MemberDTO ToDTO(this MemberVM vm)
        {
            return new MemberDTO
            {
                MemberID = vm.MemberID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                Address = vm.Address,
                Branch = vm.Branch,
                ContactNo = vm.ContactNo,
                EmailAddress = vm.EmailAddress
            };
        }

        // Converts a collection of DTOs → ViewModels
        public static List<MemberVM> ToViewModels(this IEnumerable<MemberDTO> dtos)
        {
            return dtos.Select(ToViewModel).ToList();
        }
    }
}

/* HOW IT WORKS:
  This static class serves as the mapping bridge specifically between the Web Layer 
  (ViewModels) and the Application Layer (DTOs). 

  1. UI ISOLATION: By having a separate MemberVM and this mapper, you ensure that 
     changes to the User Interface (like adding a 'ConfirmPassword' field or a 
     'FullName' property) don't force you to change your Application or Domain layers.

  2. EXTENSION METHODS: The use of the 'this' keyword (e.g., 'this MemberDTO dto') 
     allows these methods to be called directly on the objects. In your Controller, 
     you can simply write 'myDto.ToViewModel()', making the code very clean.

  3. TWO-WAY TRANSLATION:
     - ToViewModel: Converts data coming FROM the server to a format ready for 
       the View (.cshtml files).
     - ToDTO: Converts data coming FROM a submitted form back into an object 
       that the business logic Handlers can understand.

  4. BULK CONVERSION: The 'ToViewModels' method uses LINQ (.Select) to transform 
     an entire collection of DTOs into a list of ViewModels at once. This is 
     primarily used for the Index page to display all members in a table.

  5. TYPE CONSISTENCY: It ensures that specific types (like nullable DateTimes or 
     Strings) are moved correctly between the layers without data loss.
*/