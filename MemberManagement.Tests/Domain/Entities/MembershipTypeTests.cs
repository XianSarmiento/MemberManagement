using FluentAssertions;
using MemberManagement.Domain.Entities;
using Xunit;
using System;

namespace MemberManagement.UnitTests.Domain.Entities
{
    public class MembershipTypeTests
    {
        [Fact]
        public void Update_WithValidData_ShouldModifyProperties()
        {
            // Arrange
            var type = new MembershipType("Regular Member", "REG", 50m);

            // Act
            type.Update("Associate Member", "ASSOC", 75m, "Associate membership tier");

            // Assert
            type.Name.Should().Be("Associate Member");
            type.MembershipCode.Should().Be("ASSOC");
            type.MembershipFee.Should().Be(75m);
            type.Description.Should().Be("Associate membership tier");
        }

        [Fact]
        public void Constructor_WhenNameIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new MembershipType(null!, "REG", 50m);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_Should_Create_Regular_Member_Type()
        {
            var type = new MembershipType("Regular Member", "REG", 50m, "Standard membership");

            type.Name.Should().Be("Regular Member");
            type.MembershipCode.Should().Be("REG");
            type.MembershipFee.Should().Be(50m);
            type.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Constructor_Should_Create_BalikSagip_Member_Type()
        {
            var type = new MembershipType("Balik-Sagip Member", "BSM", 0m, "Special returnee program");

            type.Name.Should().Be("Balik-Sagip Member");
            type.MembershipCode.Should().Be("BSM");
            type.MembershipFee.Should().Be(0m);
        }

        [Fact]
        public void Constructor_Should_Create_Extension_Member_Type()
        {
            var type = new MembershipType("Extension Member", "EXT", 25m, "Extension program member");

            type.Name.Should().Be("Extension Member");
            type.MembershipCode.Should().Be("EXT");
            type.MembershipFee.Should().Be(25m);
        }
    }
}