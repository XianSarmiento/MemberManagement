using FluentAssertions;
using MemberManagement.Application.DTOs;
using Xunit;

namespace MemberManagement.Tests.Application.DTOs;

public class MembershipTypeDtoTests
{
    [Fact]
    public void MembershipTypeDto_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var dto = new MembershipTypeDto
        {
            Id = 1,
            Name = "Gold",
            MembershipFee = 1500.00m,
            IsActive = true
        };

        // Assert
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Gold");
        dto.MembershipFee.Should().Be(1500.00m);
    }
}