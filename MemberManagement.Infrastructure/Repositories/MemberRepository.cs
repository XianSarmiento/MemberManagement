using MemberManagement.Domain;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberManagement.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MMSDbContext _context;

        public MemberRepository(MMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            // only active members
            return await _context.Members.Where(m => m.IsActive).ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int memberId)
        {
            return await _context.Members.FindAsync(memberId);
        }

        public async Task AddAsync(Member member)
        {
            member.IsActive = true; // ensure active by default
            member.DateCreated = DateTime.Now;
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member != null)
            {
                member.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
