using FluentAssertions;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MemberManagement.Tests.Application.Validation
{
    public class BranchValidatorTests
    {
        private readonly MMSDbContext _context;

        public BranchValidatorTests()
        {
            var options = new DbContextOptionsBuilder<MMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new MMSDbContext(options);
        }

        [Fact]
        public async Task BranchValidator_DuplicateCode_ShouldFail()
        {
            // Arrange
            var code = "B001";
            _context.Branches.Add(new Branch("Original", "Address", code));
            await _context.SaveChangesAsync();

            var validator = new BranchValidator(_context);
            var duplicateBranch = new Branch("New", "Address", code);

            // Act
            var result = await validator.ValidateAsync(duplicateBranch);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("already exists"));
        }
    }
}