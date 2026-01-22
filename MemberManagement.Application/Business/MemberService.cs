using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Member> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Member member)
        {
            // Business rules go here
            member.IsActive = true;
            member.DateCreated = DateTime.Now;

            await _repository.AddAsync(member);
        }

        public async Task UpdateAsync(Member member)
        {
            await _repository.UpdateAsync(member);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }
    }
}
