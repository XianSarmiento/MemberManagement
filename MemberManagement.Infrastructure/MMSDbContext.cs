using MemberManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Infrastructure
{
    public class MMSDbContext : DbContext
    {
        public MMSDbContext(DbContextOptions<MMSDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
    }
}
