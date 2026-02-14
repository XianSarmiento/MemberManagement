using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class MembershipTypeService : IMembershipTypeService
{
    private readonly MMSDbContext _context;

    public MembershipTypeService(MMSDbContext context) => _context = context;

    public async Task<IEnumerable<MembershipType>> GetAllAsync()
    {
        return await _context.MembershipTypes.AsNoTracking().ToListAsync();
    }
}