using FluentAssertions;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MemberManagement.Test.Services
{
    public class BranchServiceTests
    {
        private MMSDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MMSDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBranches()
        {
            // Arrange
            var context = GetDbContext();

            // Fix: Use the constructor defined in your Branch entity
            // signature: Branch(string name, string address, string branchCode)
            var branches = new List<Branch>
            {
                new Branch("Main Branch", "123 Main St", "BR001"),
                new Branch("West Coast", "456 West Ave", "BR002")
            };

            context.Branches.AddRange(branches);
            await context.SaveChangesAsync();

            var service = new BranchService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(b => b.Name == "Main Branch" && b.BranchCode == "BR001");
            result.Should().Contain(b => b.Name == "West Coast" && b.BranchCode == "BR002");
        }

        [Fact]
        public async Task GetAllAsync_WhenEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var context = GetDbContext();
            var service = new BranchService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}