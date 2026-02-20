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

            // Use your own branches
            var branches = new List<Branch>
            {
                new Branch("Catanduanes Branch", "123 Catanduanes St", "R01"),
                new Branch("Albay Branch", "456 Albay Ave", "A01"),
                new Branch("Sorsogon Branch", "789 Sorsogon Blvd", "B01"),
                new Branch("Camarines Sur Branch", "321 Camarines St", "E01")
            };

            context.Branches.AddRange(branches);
            await context.SaveChangesAsync();

            var service = new BranchService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(4);
            result.Should().Contain(b => b.Name == "Catanduanes Branch" && b.BranchCode == "R01");
            result.Should().Contain(b => b.Name == "Albay Branch" && b.BranchCode == "A01");
            result.Should().Contain(b => b.Name == "Sorsogon Branch" && b.BranchCode == "B01");
            result.Should().Contain(b => b.Name == "Camarines Sur Branch" && b.BranchCode == "E01");
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