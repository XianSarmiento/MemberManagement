using MemberManagement.Application.Business;
using MemberManagement.Web.ViewModels;

namespace MemberManagement.Web.Mappers
{
    public static class MemberMapper
    {
        // Convert DTO → ViewModel
        public static MemberVM ToViewModel(this MemberDTO memberDto)
        {
            if (memberDto == null) return null;

            return new MemberVM
            {
                MemberID = memberDto.MemberID,
                FirstName = memberDto.FirstName,
                LastName = memberDto.LastName,
                BirthDate = memberDto.BirthDate,
                Address = memberDto.Address,
                Branch = memberDto.Branch,
                ContactNo = memberDto.ContactNo,
                EmailAddress = memberDto.EmailAddress,
                IsActive = memberDto.IsActive,
                DateCreated = memberDto.DateCreated
            };
        }

        // Convert IEnumerable<DTO> → List<ViewModel>
        public static List<MemberVM> ToViewModels(this IEnumerable<MemberDTO> memberDtos) =>
            memberDtos?.Select(d => d.ToViewModel()).ToList() ?? new List<MemberVM>();

        // Convert ViewModel → DTO
        public static MemberDTO ToDTO(this MemberVM memberVm)
        {
            if (memberVm == null) return null;

            return new MemberDTO
            {
                MemberID = memberVm.MemberID,
                FirstName = memberVm.FirstName,
                LastName = memberVm.LastName,
                BirthDate = memberVm.BirthDate,

                // Optional fields: convert empty strings to null
                Address = string.IsNullOrWhiteSpace(memberVm.Address) ? null : memberVm.Address.Trim(),
                Branch = string.IsNullOrWhiteSpace(memberVm.Branch) ? null : memberVm.Branch.Trim(),
                ContactNo = string.IsNullOrWhiteSpace(memberVm.ContactNo) ? null : memberVm.ContactNo.Trim(),
                EmailAddress = string.IsNullOrWhiteSpace(memberVm.EmailAddress) ? null : memberVm.EmailAddress.Trim(),

                IsActive = memberVm.IsActive,
                DateCreated = memberVm.DateCreated
            };
        }
    }
}
