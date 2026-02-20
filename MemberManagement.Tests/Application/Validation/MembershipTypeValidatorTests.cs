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
        private readonly MembershipTypeValidator _validator;

        public MembershipTypeValidatorTests()
        {
            var options = new DbContextOptionsBuilder<MMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MMSDbContext(options);
            _validator = new MembershipTypeValidator(_context);
        }

        private MembershipType CreateMembershipType(string name, string code, decimal fee)
        {
            return new MembershipType(name, code, fee);
        }

        [Theory]
        [InlineData("Regular Member", "REG", 50)]
        [InlineData("Associate Member", "ASSOC", 50)]
        [InlineData("Balik-Sagip Member", "BSM", 30)]
        [InlineData("Extension Member", "EXT", 20)]
        public async Task ValidMembershipType_ShouldPass(string name, string code, decimal fee)
        {
            // Arrange
            var type = CreateMembershipType(name, code, fee);

            // Act
            var result = await _validator.ValidateAsync(type);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task DuplicateName_ShouldFail()
        {
            // Arrange
            var existing = CreateMembershipType("Regular Member", "REG", 50);
            _context.MembershipTypes.Add(existing);
            await _context.SaveChangesAsync();

            var duplicate = CreateMembershipType("Regular Member", "REG2", 60);

            // Act
            var result = await _validator.ValidateAsync(duplicate);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Membership name already exists"));
        }

        [Fact]
        public async Task DuplicateCode_ShouldFail()
        {
            // Arrange
            var existing = CreateMembershipType("Regular Member", "REG", 50);
            _context.MembershipTypes.Add(existing);
            await _context.SaveChangesAsync();

            var duplicate = CreateMembershipType("Associate Member", "REG", 60);

            // Act
            var result = await _validator.ValidateAsync(duplicate);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("Membership code already exists"));
        }

        [Fact]
        public async Task NegativeFee_ShouldFail()
        {
            var type = CreateMembershipType("Balik-Sagip Member", "BSM", -10);

            var result = await _validator.ValidateAsync(type);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.ErrorMessage.Contains("cannot be negative"));
        }

        [Fact]
        public async Task EmptyNameOrCode_ShouldFail()
        {
            var type1 = CreateMembershipType("", "EXT", 20);
            var type2 = CreateMembershipType("Extension Member", "", 20);

            var result1 = await _validator.ValidateAsync(type1);
            var result2 = await _validator.ValidateAsync(type2);

            result1.IsValid.Should().BeFalse();
            result1.Errors.Should().Contain(e => e.PropertyName == "Name");

            result2.IsValid.Should().BeFalse();
            result2.Errors.Should().Contain(e => e.PropertyName == "MembershipCode");
        }
    }
}