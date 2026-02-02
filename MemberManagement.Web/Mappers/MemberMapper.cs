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
                Address = vm.Address,
                Branch = vm.Branch,
                ContactNo = vm.ContactNo,
                EmailAddress = vm.EmailAddress,
                IsActive = vm.IsActive,
                DateCreated = vm.DateCreated
            };
        }
    }
}
