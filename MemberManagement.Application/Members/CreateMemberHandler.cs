using FluentValidation;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Mappers;
using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Members;

public class CreateMemberHandler
{
    private readonly IMemberService _memberService;
    private readonly IValidator<Member> _validator;

    public CreateMemberHandler(IMemberService memberService, IValidator<Member> validator)
    {
        _memberService = memberService;
        _validator = validator;
    }

    public async Task HandleAsync(MemberDTO dto)
    {
        var member = dto.ToEntity();

        var result = await _validator.ValidateAsync(member);

        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        await _memberService.CreateAsync(member);
    }
}