using MemberManagement.Application.DTOs;
using MemberManagement.Application.Mappers;
using MemberManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace MemberManagement.Tests.Application.Mappers;

public class MemberEntityMapperTests
{
    [Fact]
    public void ToDto_ShouldMapEntityToDto_Correctly()
    {
        // Arrange
        var birthDate = new DateOnly(1997, 8, 7);

        var member = new Member(
            "John Christian",
            "Sarmiento",
            birthDate,
            10,
            5,
            "Naga City",
            "09123456789",
            "john.sarmiento@email.com"
        )
        {
            MemberID = 123,
            IsActive = true,
            DateCreated = DateTime.Now
        };

        // Act
        var dto = member.ToDto();

        // Assert
        dto.MemberID.Should().Be(member.MemberID);
        dto.FirstName.Should().Be("John Christian");
        dto.LastName.Should().Be("Sarmiento");
        dto.BirthDate.Should().Be(birthDate.ToDateTime(TimeOnly.MinValue));
        dto.BranchId.Should().Be(10);
        dto.EmailAddress.Should().Be("john.sarmiento@email.com");
    }

    [Fact]
    public void ToEntity_ShouldMapDtoToEntity_Correctly()
    {
        // Arrange
        var testDate = new DateTime(1997, 8, 7);

        var dto = new MemberDTO
        {
            MemberID = 99,
            FirstName = "John Christian",
            LastName = "Sarmiento",
            BirthDate = testDate,
            BranchId = 1,
            MembershipTypeId = 2,
            Address = "Naga City",
            EmailAddress = "john.sarmiento@email.com"
        };

        // Act
        var entity = dto.ToEntity();

        // Assert
        entity.MemberID.Should().Be(dto.MemberID);
        entity.FirstName.Should().Be("John Christian");
        entity.LastName.Should().Be("Sarmiento");
        entity.BirthDate.Should().Be(DateOnly.FromDateTime(testDate));
        entity.BranchId.Should().Be(dto.BranchId);
        entity.MembershipTypeId.Should().Be(dto.MembershipTypeId);
    }

    [Fact]
    public void ToDto_ShouldHandleNullNavigationProperties()
    {
        // Arrange
        var member = new Member(
            "John Christian",
            "Sarmiento",
            new DateOnly(1997, 8, 7),
            1,
            1,
            null,
            null,
            null
        );

        // Act
        var dto = member.ToDto();

        // Assert
        dto.Branch.Should().Be("N/A");
        dto.MembershipType.Should().Be("N/A");
    }
}