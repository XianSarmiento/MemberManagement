using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MemberManagement.Infrastructure;

public class BranchService : IBranchService
{
    private readonly MMSDbContext _context;

    public BranchService(MMSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Branch>> GetAllAsync()
    {
        return await _context.Branches
            .AsNoTracking()
            .ToListAsync();
    }
}