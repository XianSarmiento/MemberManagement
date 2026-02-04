using MemberManagement.Application.Business;
using MemberManagement.Web.ViewModels;

namespace MemberManagement.Web.Mappers
{
    public static class MemberMapper
    {
        // DTO → VM
        public static MemberVM ToViewModel(this MemberDTO dto)
        {
            if (dto == null) return null;

            return new MemberVM
            {
                MemberID = dto.MemberID,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Branch = dto.Branch,
                ContactNo = dto.ContactNo,
                EmailAddress = dto.EmailAddress,
                IsActive = dto.IsActive,
                DateCreated = dto.DateCreated
            };
        }

        public static List<MemberVM> ToViewModels(this IEnumerable<MemberDTO> dtos)
        {
            return dtos?.Select(d => d.ToViewModel()).ToList() ?? new List<MemberVM>();
        }

        // VM → DTO
        public static MemberDTO ToDTO(this MemberVM vm)
        {
            if (vm == null) return null;

            return new MemberDTO
            {
                MemberID = vm.MemberID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,

                // Convert empty strings to null for optional fields
                Address = string.IsNullOrWhiteSpace(vm.Address) ? null : vm.Address.Trim(),
                Branch = string.IsNullOrWhiteSpace(vm.Branch) ? null : vm.Branch.Trim(),
                ContactNo = string.IsNullOrWhiteSpace(vm.ContactNo) ? null : vm.ContactNo.Trim(),
                EmailAddress = string.IsNullOrWhiteSpace(vm.EmailAddress) ? null : vm.EmailAddress.Trim(),

                IsActive = vm.IsActive,
                DateCreated = vm.DateCreated
            };
        }
    }
}
