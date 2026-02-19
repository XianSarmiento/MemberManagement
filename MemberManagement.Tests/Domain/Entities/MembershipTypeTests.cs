using FluentAssertions;
using MemberManagement.Domain.Entities;
using Xunit;

namespace MemberManagement.UnitTests.Domain.Entities
{
    public class MembershipTypeTests
    {
        [Fact]
        public void Update_WithValidData_ShouldModifyProperties()
        {
            // Arrange
            var type = new MembershipType("Silver", "S01", 100m);

            // Act
            type.Update("Gold", "G01", 500m, "Premium tier");

            // Assert
            type.Name.Should().Be("Gold");
            type.MembershipFee.Should().Be(500m);
            type.Description.Should().Be("Premium tier");
        }

        [Fact]
        public void Constructor_WhenNameIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new MembershipType(null!, "CODE", 100m);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}