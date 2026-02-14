using FluentValidation;
using MemberManagement.Application.Core;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Application.Members
{
    public class UpdateMemberHandler
    {
        private readonly IMemberService _memberService;
        private readonly IValidator<Member> _validator;

        public UpdateMemberHandler(IMemberService memberService, IValidator<Member> validator)
        {
            _memberService = memberService;
            _validator = validator;
        }

        public async Task HandleAsync(MemberDTO dto)
        {
            var member = await _memberService.GetByIdAsync(dto.MemberID)
                ?? throw new KeyNotFoundException(OperationMessage.Error.NotFound);

            UpdateMemberFields(member, dto);

            var result = await _validator.ValidateAsync(member);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _memberService.UpdateAsync(member);
        }

        private void UpdateMemberFields(Member member, MemberDTO dto)
        {
            member.FirstName = dto.FirstName!;
            member.LastName = dto.LastName!;
            member.BirthDate = dto.BirthDate.HasValue
                ? DateOnly.FromDateTime(dto.BirthDate.Value)
                : null;
            member.Address = dto.Address;
            member.ChangeBranch(dto.BranchId);
            member.ContactNo = dto.ContactNo;
            member.EmailAddress = dto.EmailAddress;
        }
    }
}

