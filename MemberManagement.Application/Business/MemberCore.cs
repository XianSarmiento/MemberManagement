using MemberManagement.Application.Business;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagement.Application.Core
{
    // Application-layer orchestration for member-related operations
    public class MemberCore
    {
        private readonly IMemberService _memberService;
        private readonly IValidator<Member> _validator;

        // Maps domain Member entity to MemberDTO
        private static MemberDTO MapToDto(Member m) => new()
        {
            MemberID = m.MemberID,
            FirstName = m.FirstName,
            LastName = m.LastName,
            BirthDate = m.BirthDate.ToDateTime(TimeOnly.MinValue),
            Address = m.Address,
            Branch = m.Branch,
            ContactNo = m.ContactNo,
            EmailAddress = m.EmailAddress,
            IsActive = m.IsActive,
            DateCreated = m.DateCreated
        };

        public MemberCore(IMemberService memberService, IValidator<Member> validator)
        {
            _memberService = memberService;
            _validator = validator;
        }

        // Get all active members as DTO
        public async Task<IEnumerable<MemberDTO>> GetActiveMembersAsync()
        {
            var members = await _memberService.GetActiveMembersAsync();

            return members.Select(MapToDto);
        }

        public async Task<MemberDTO?> GetMemberByIdAsync(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return null;

            return MapToDto(member);
        }

        // Create a new member using DTO
        public async Task<string> CreateMemberAsync(MemberDTO dto)
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

            ValidationResult result = _validator.Validate(member);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            await _memberService.CreateAsync(member);
            return OperationMessage.User.Created;
        }

        // Update existing member using DTO
        public async Task<string> UpdateMemberAsync(MemberDTO dto)
        {
            var member = await _memberService.GetByIdAsync(dto.MemberID);
            if (member == null)
                throw new KeyNotFoundException(OperationMessage.Error.NotFound);

            // Map DTO → Entity
            member.FirstName = dto.FirstName!;
            member.LastName = dto.LastName!;
            member.BirthDate = DateOnly.FromDateTime(dto.BirthDate.Date);
            member.Address = dto.Address!;
            member.Branch = dto.Branch!;
            member.ContactNo = dto.ContactNo!;
            member.EmailAddress = dto.EmailAddress!;

            // Ensure updated member data is valid before saving
            ValidationResult result = _validator.Validate(member);
            if (!result.IsValid)
            {
                throw new ValidationException(OperationMessage.Error.InvalidInput, result.Errors);
            }

            await _memberService.UpdateAsync(member);
            return OperationMessage.User.Updated;
        }

        // Soft delete
        public async Task<string> DeleteMemberAsync(int id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
                throw new KeyNotFoundException(OperationMessage.Error.NotFound);

            member.IsActive = false;
            await _memberService.UpdateAsync(member);
            return OperationMessage.User.Deleted;
        }

        // Get members for index with filtering, sorting, and pagination
        public async Task<MemberIndexResult> GetMembersForIndexAsync(
            string searchLastName, string branch, 
            string sortColumn = "MemberId", string sortOrder = "asc") 
        {
            var dtos = (await GetActiveMembersAsync()).ToList();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchLastName))
                dtos = dtos.Where(d => d.LastName?.Contains(searchLastName, StringComparison.OrdinalIgnoreCase) ?? false).ToList();

            if (!string.IsNullOrWhiteSpace(branch))
                dtos = dtos.Where(d => d.Branch?.Equals(branch, StringComparison.OrdinalIgnoreCase) ?? false).ToList();

            // Sorting
            dtos = (sortColumn, sortOrder.ToLower()) switch
            {
                ("MemberID", "asc") => dtos.OrderBy(d => d.MemberID).ToList(),
                ("MemberID", "desc") => dtos.OrderByDescending(d => d.MemberID).ToList(),
                ("FirstName", "asc") => dtos.OrderBy(d => d.FirstName).ToList(),
                ("FirstName", "desc") => dtos.OrderByDescending(d => d.FirstName).ToList(),
                ("LastName", "asc") => dtos.OrderBy(d => d.LastName).ToList(),
                ("LastName", "desc") => dtos.OrderByDescending(d => d.LastName).ToList(),
                ("BirthDate", "asc") => dtos.OrderBy(d => d.BirthDate).ToList(),
                ("BirthDate", "desc") => dtos.OrderByDescending(d => d.BirthDate).ToList(),
                ("Branch", "asc") => dtos.OrderBy(d => d.Branch).ToList(),
                ("Branch", "desc") => dtos.OrderByDescending(d => d.Branch).ToList(),
                _ => dtos.OrderBy(d => d.MemberID).ToList()
            };

            return new MemberIndexResult
            {
                Members = dtos,
                Branches = dtos.Select(d => d.Branch!).Where(b => !string.IsNullOrWhiteSpace(b)).Distinct().OrderBy(b => b).ToList(),
                TotalItems = dtos.Count
            };
        }

    }
}
