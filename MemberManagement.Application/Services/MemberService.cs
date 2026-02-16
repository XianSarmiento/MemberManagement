using FluentValidation;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IValidator<Member> _validator;

        public MemberService(IMemberRepository repository, IValidator<Member> validator)
        {
            _memberRepository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Member>> GetInactiveMembersAsync()
        {
            var allMembers = await _memberRepository.GetAllAsync(onlyActive: false);
            return allMembers.Where(m => !m.IsActive);
        }

        public async Task RestoreAsync(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            if (member != null)
            {
                member.IsActive = true;
                await _memberRepository.UpdateAsync(member);
            }
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

            member.Initialize();

            await _memberRepository.AddAsync(member);
        }

        public async Task UpdateAsync(Member member)
        {
            var result = await _validator.ValidateAsync(member);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            await _memberRepository.UpdateAsync(member);
        }

        public Task DeleteAsync(int id)
        {
            return _memberRepository.SoftDeleteAsync(id);
        }
    }
}