using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace MemberManagement.Application.Services
{
    public class MemberService(IMemberRepository repository) : IMemberService
    {
        private readonly IMemberRepository _repository =
            repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<IEnumerable<Member>> GetActiveMembersAsync()
        {
            var all = await _repository.GetAllAsync();
            return all.Where(m => m.IsActive); // ensure only active members
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Member member)
        {
            // Validation using FluentValidation
            var validator = new MemberValidator();
            FluentValidation.Results.ValidationResult result = validator.Validate(member);

            if (!result.IsValid)
            {
                throw new FluentValidation.ValidationException(result.Errors);
            }

            // Business rules go here
            member.IsActive = true;
            member.DateCreated = DateTime.Now;

            await _repository.AddAsync(member);
        }

        public async Task UpdateAsync(Member member)
        {
            var validator = new MemberValidator();
            FluentValidation.Results.ValidationResult result = validator.Validate(member);

            if (!result.IsValid)
            {
                throw new FluentValidation.ValidationException(result.Errors);
            }

            await _repository.UpdateAsync(member);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }
    }
}
