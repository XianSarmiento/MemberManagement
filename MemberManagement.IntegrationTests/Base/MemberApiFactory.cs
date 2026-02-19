using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using MemberManagement.Web;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;

namespace MemberManagement.IntegrationTests.Base;

public class MemberApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Remove the real Database context
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<MMSDbContext>));

            if (descriptor != null) services.Remove(descriptor);

            // 2. Add In-Memory Database
            services.AddDbContext<MMSDbContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTestDb");
            });

            // 3. BYPASS ANTI-FORGERY
            services.AddSingleton<IAntiforgery, FakeAntiforgery>();

            // 4. SEEDING LOGIC
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MMSDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            SeedDatabase(db);
        });
    }

    // IDE FIX: Marked as static because it doesn't use instance members
    private static void SeedDatabase(MMSDbContext db)
    {
        db.Branches.Add(new Branch("Main Office", "123 Tech St", "MAIN01"));
        db.MembershipTypes.Add(new MembershipType("Regular", "REG01", 500m));
        db.SaveChanges();
    }
}

// --- FAKE ANTIFORGERY CLASS ---
public class FakeAntiforgery : IAntiforgery
{
    // IDE FIX: 'new' expression simplified to target-typed new
    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext)
        => new("f-token", "f-cookie", "f-field", "f-header");

    public AntiforgeryTokenSet GetTokens(HttpContext httpContext)
        => new("f-token", "f-cookie", "f-field", "f-header");

    public Task<bool> IsRequestValidAsync(HttpContext httpContext)
        => Task.FromResult(true);

    public Task ValidateRequestAsync(HttpContext httpContext)
        => Task.CompletedTask;

    public void SetCookieTokenAndHeader(HttpContext httpContext) { }
}