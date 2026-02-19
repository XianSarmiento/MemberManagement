using FluentAssertions;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using MemberManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MemberManagement.UnitTests.Validation
{
    public class MembershipTypeValidatorTests
    {
        private readonly MMSDbContext _context;

        public MembershipTypeValidatorTests()
        {
            var options = new DbContextOptionsBuilder<MMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new MMSDbContext(options);
        }

        [Fact]
        public async Task MembershipTypeValidator_DuplicateCode_ShouldFail()
        {
            // Arrange
            var code = "GOLD";
            _context.MembershipTypes.Add(new MembershipType("Old Gold", code, 100m));
            await _context.SaveChangesAsync();

            var validator = new MembershipTypeValidator(_context);
            var duplicateType = new MembershipType("New Gold", code, 200m);

            // Act
            var result = await validator.ValidateAsync(duplicateType);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("already exists"));
        }
    }
}