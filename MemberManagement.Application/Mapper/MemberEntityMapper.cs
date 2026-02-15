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
                BranchId = member.BranchId,
                MembershipTypeId = member.MembershipTypeId,
                Branch = member.Branch?.Name ?? "N/A",
                MembershipType = member.MembershipType?.Name ?? "N/A",
                Address = member.Address,
                ContactNo = member.ContactNo,
                EmailAddress = member.EmailAddress,
                IsActive = member.IsActive,
                DateCreated = member.DateCreated
            };
        }

        // Converts MemberDTO → Member (Entity)
        public static Member ToEntity(this MemberDTO dto)
        {
            DateOnly? birthDateOnly = dto.BirthDate.HasValue
                ? DateOnly.FromDateTime(dto.BirthDate.Value)
                : null;

            var member = new Member(
                dto.FirstName ?? string.Empty,
                dto.LastName ?? string.Empty,
                birthDateOnly,
                dto.BranchId,
                dto.MembershipTypeId,
                dto.Address,
                dto.ContactNo,
                dto.EmailAddress
            );

            member.MemberID = dto.MemberID;
            return member;
        }
    }
}