using FluentValidation;
using FluentValidation.Results;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagement.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IValidator<Member> _validator;

        public MemberService(IMemberRepository repository, IValidator<Member> validator)
        {
            _memberRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            // Ideally, move filtering to repository
            var allMembers = await _memberRepository.GetAllAsync();
            return allMembers.Where(member => member.IsActive);
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Member member)
        {
            var result = await _validator.ValidateAsync(member);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            member.IsActive = true;
            member.DateCreated = DateTime.UtcNow;

            await _memberRepository.AddAsync(member);
        }

        public async Task UpdateAsync(Member member)
        {
            var result = await _validator.ValidateAsync(member);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _memberRepository.UpdateAsync(member);
        }

        public async Task DeleteAsync(int id)
        {
            await _memberRepository.SoftDeleteAsync(id);
        }
    }
}
