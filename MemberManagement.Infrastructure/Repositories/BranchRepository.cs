using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Infrastructure.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly MMSDbContext _context;

        public BranchRepository(MMSDbContext context)
        {
            _context = context;
        }

        public async Task<Branch?> GetByIdAsync(int id) =>
            await _context.Branches.FindAsync(id);

        public async Task<IEnumerable<Branch>> GetAllAsync() =>
            await _context.Branches.ToListAsync();

        public async Task AddAsync(Branch branch)
        {
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Branch branch)
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                branch.Deactivate();
                await _context.SaveChangesAsync();
            }
        }
    }
}
