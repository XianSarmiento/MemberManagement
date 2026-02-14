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
                BranchId = member.BranchId,
                ContactNo = member.ContactNo,
                EmailAddress = member.EmailAddress,
                IsActive = member.IsActive,
                DateCreated = member.DateCreated
            };
        }

        // Converts MemberDTO → Member (Entity)
        public static Member ToEntity(this MemberDTO dto)
        {
            var member = new Member(
                dto.FirstName!,
                dto.LastName!,
                DateOnly.FromDateTime(dto.BirthDate!.Value),
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

