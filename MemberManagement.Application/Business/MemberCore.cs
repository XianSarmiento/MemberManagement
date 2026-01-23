using MemberManagement.Application.Business;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using MemberManagement.Application.Validation;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MemberManagement.Application.Core
{
    public class MemberCore
    {
        private readonly IMemberService _memberService;
        private readonly IValidator<Member> _validator;

        public MemberCore(IMemberService memberService, IValidator<Member> validator)
        {
            _memberService = memberService;
            _validator = validator;
        }

        // Get all active members as DTO
        public async Task<IEnumerable<MemberDTO>> GetActiveMembersAsync()
        {
            var members = await _memberService.GetActiveMembersAsync();

            return members.Select(m => new MemberDTO
            {
                MemberID = m.MemberID,
                FirstName = m.FirstName,
                LastName = m.LastName,
                BirthDate = m.BirthDate.ToDateTime(TimeOnly.MinValue),
                Address = m.Address,
                Branch = m.Branch,
                ContactNo = m.ContactNo,
                EmailAddress = m.EmailAddress
            });
        }

        public async Task<MemberDTO?> GetMemberByIdAsync(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return null;

            return new MemberDTO
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate.ToDateTime(TimeOnly.MinValue),
                Address = member.Address,
                Branch = member.Branch,
                ContactNo = member.ContactNo,
                EmailAddress = member.EmailAddress
            };
        }

        // Create a new member using DTO
        public async Task CreateMemberAsync(MemberDTO dto)
        {
            // Map DTO → Entity
            var member = new Member
            {
                FirstName = dto.FirstName!,
                LastName = dto.LastName!,
                BirthDate = DateOnly.FromDateTime(dto.BirthDate.Date),
                Address = dto.Address!,
                Branch = dto.Branch!,
                ContactNo = dto.ContactNo!,
                EmailAddress = dto.EmailAddress!
            };

            // Validate entity
            ValidationResult result = _validator.Validate(member);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _memberService.CreateAsync(member);
        }

        // Update existing member using DTO
        public async Task UpdateMemberAsync(MemberDTO dto)
        {
            var member = await _memberService.GetByIdAsync(dto.MemberID);
            if (member == null)
                throw new Exception("Member not found.");

            // Map DTO → Entity
            member.FirstName = dto.FirstName!;
            member.LastName = dto.LastName!;
            member.BirthDate = DateOnly.FromDateTime(dto.BirthDate.Date);
            member.Address = dto.Address!;
            member.Branch = dto.Branch!;
            member.ContactNo = dto.ContactNo!;
            member.EmailAddress = dto.EmailAddress!;

            // Validate updated entity
            ValidationResult result = _validator.Validate(member);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _memberService.UpdateAsync(member);
        }

        // Soft delete
        public async Task DeleteMemberAsync(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
                throw new Exception("Member not found.");

            member.IsActive = false;
            await _memberService.UpdateAsync(member);
        }
    }
}
