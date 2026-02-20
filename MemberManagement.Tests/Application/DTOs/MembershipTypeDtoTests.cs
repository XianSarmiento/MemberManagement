using FluentAssertions;
using MemberManagement.Application.DTOs;
using Xunit;

namespace MemberManagement.Tests.Application.DTOs;

public class MembershipTypeDtoTests
{
    [Fact]
    public void MembershipTypeDto_ShouldSet_RegularMember_Correctly()
    {
        // Arrange & Act
        var dto = new MembershipTypeDto
        {
            Id = 1,
            Name = "Regular Member",
            MembershipFee = 50.00m,
            IsActive = true
        };

        // Assert
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Regular Member");
        dto.MembershipFee.Should().Be(50.00m);
        dto.IsActive.Should().BeTrue();
    }

    [Fact]
    public void MembershipTypeDto_ShouldSet_AssociateMember_Correctly()
    {
        var dto = new MembershipTypeDto
        {
            Id = 2,
            Name = "Associate Member",
            MembershipFee = 75.00m,
            IsActive = true
        };

        dto.Name.Should().Be("Associate Member");
        dto.MembershipFee.Should().Be(75.00m);
    }

    [Fact]
    public void MembershipTypeDto_ShouldSet_BalikSagipMember_Correctly()
    {
        var dto = new MembershipTypeDto
        {
            Id = 3,
            Name = "Balik-Sagip Member",
            MembershipFee = 0.00m,
            IsActive = true
        };

        dto.Name.Should().Be("Balik-Sagip Member");
        dto.MembershipFee.Should().Be(0.00m);
    }

    [Fact]
    public void MembershipTypeDto_ShouldSet_ExtensionMember_Correctly()
    {
        var dto = new MembershipTypeDto
        {
            Id = 4,
            Name = "Extension Member",
            MembershipFee = 25.00m,
            IsActive = true
        };

        dto.Name.Should().Be("Extension Member");
        dto.MembershipFee.Should().Be(25.00m);
    }
}