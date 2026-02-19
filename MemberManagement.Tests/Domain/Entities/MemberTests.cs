using MemberManagement.Domain.Entities;
using MemberManagement.SharedKernel.Constant;
using FluentAssertions;
using Xunit;
using System; // FIX: Added for Action delegate

namespace MemberManagement.UnitTests.Domain.Entities
{
    public class MemberTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldInitializeCorrectly()
        {
            // Arrange
            var birthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-25);

            // Act
            var member = new Member("John", "Doe", birthDate, 1, 1, "Manila", "09123456789", "john@test.com");

            // Assert
            member.FirstName.Should().Be("John");
            member.IsActive.Should().BeTrue();
            // BeCloseTo is great for handling the slight delay in execution time
            member.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Constructor_WhenBirthDateNull_ShouldThrowException()
        {
            // Act
            // Use the ! (null-forgiving operator) to tell the compiler you are intentionally passing null
            Action act = () => new Member("John", "Doe", null!, 1, 1);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage(OperationMessage.Error.BirthDateRequired);
        }

        [Theory]
        [InlineData(-17)]
        [InlineData(-5)]
        public void Constructor_WhenUnderage_ShouldThrowException(int yearsToAdd)
        {
            // Arrange
            var birthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(yearsToAdd);

            // Act
            Action act = () => new Member("John", "Doe", birthDate, 1, 1);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage(OperationMessage.Error.Underage);
        }

        [Fact]
        public void Constructor_WhenExceedsAgeLimit_ShouldThrowException()
        {
            // Arrange: 70 years ago
            var birthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-70);

            // Act
            Action act = () => new Member("Old", "User", birthDate, 1, 1);

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage(OperationMessage.Error.ExceedsAgeLimit);
        }

        [Fact]
        public void Initialize_ShouldResetState()
        {
            // Arrange 
            // If IsActive has a public setter, this works. 
            // If it's private, you must use the parameterless constructor 
            // and trust the default state or use Reflection.
            var member = new Member();

            // Act
            member.Initialize();

            // Assert
            member.IsActive.Should().BeTrue();
            member.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}