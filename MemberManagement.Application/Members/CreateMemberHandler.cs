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

/* HOW IT WORKS:
  This handler manages the process of creating a new member by coordinating three main steps:
  
  1. DEPENDENCY INJECTION: The class receives 'IMemberService' (for database operations) 
     and 'IValidator' (for business rules) via the constructor. This allows the 
     handler to stay "lean" by delegating specialized work to other services.

  2. MAPPING (DTO to Entity): It converts the incoming 'MemberDTO' into a 'Member' 
     domain entity using the '.ToEntity()' method. This ensures that the external 
     API data structure is translated into the internal business model.

  3. VALIDATION: It runs the translated entity through FluentValidation. If the 
     data violates any rules (like a missing email or duplicate ID), it throws a 
     'ValidationException'. This acts as a "guard," preventing invalid data from 
     ever reaching your database.

  4. EXECUTION/PERSISTENCE: If validation passes, it calls the MemberService 
     to save the new member. This completes the operation asynchronously.
*/