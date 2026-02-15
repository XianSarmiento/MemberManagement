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
                BranchId = dto.BranchId,
                MembershipTypeId = dto.MembershipTypeId,
                Branch = dto.Branch,
                MembershipType = dto.MembershipType,
                Address = dto.Address,
                ContactNo = dto.ContactNo,
                EmailAddress = dto.EmailAddress,
                IsActive = dto.IsActive,
                DateCreated = dto.DateCreated
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
                BranchId = vm.BranchId,
                MembershipTypeId = vm.MembershipTypeId,
                Branch = vm.Branch,
                MembershipType = vm.MembershipType,
                Address = vm.Address,
                ContactNo = vm.ContactNo,
                EmailAddress = vm.EmailAddress,
                IsActive = vm.IsActive,
                DateCreated = vm.DateCreated
            };
        }

        // Converts a collection of DTOs → ViewModels
        public static List<MemberVM> ToViewModels(this IEnumerable<MemberDTO> dtos)
        {
            return dtos.Select(ToViewModel).ToList();
        }
    }
}