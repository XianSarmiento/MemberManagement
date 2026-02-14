using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Infrastructure.Repositories
{
    public class MembershipTypeRepository : IMembershipTypeRepository
    {
        private readonly MMSDbContext _context;

        public MembershipTypeRepository(MMSDbContext context)
        {
            _context = context;
        }

        public async Task<MembershipType?> GetByIdAsync(int id) =>
            await _context.MembershipTypes.FindAsync(id);

        public async Task<IEnumerable<MembershipType>> GetAllAsync() =>
            await _context.MembershipTypes.ToListAsync();

        public async Task AddAsync(MembershipType membershipType)
        {
            await _context.MembershipTypes.AddAsync(membershipType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MembershipType membershipType)
        {
            _context.Entry(membershipType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.MembershipTypes.FindAsync(id);
            if (entity != null)
            {
                _context.MembershipTypes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}