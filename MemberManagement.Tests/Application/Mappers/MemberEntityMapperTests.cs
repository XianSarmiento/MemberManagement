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
        // Arrange: Create a real Member entity
        var birthDate = new DateOnly(1995, 5, 20);
        var member = new Member(
            "Jane",
            "Smith",
            birthDate,
            10,
            5,
            "456 Oak St",
            "09998887766",
            "jane@example.com"
        )
        {
            MemberID = 123,
            IsActive = true,
            DateCreated = DateTime.Now
        };

        // Act: Use your extension method
        var dto = member.ToDto();

        // Assert
        dto.MemberID.Should().Be(member.MemberID);
        dto.FirstName.Should().Be("Jane");
        dto.BirthDate.Should().Be(birthDate.ToDateTime(TimeOnly.MinValue));
        dto.BranchId.Should().Be(10);
        dto.EmailAddress.Should().Be("jane@example.com");
    }

    [Fact]
    public void ToEntity_ShouldMapDtoToEntity_Correctly()
    {
        // Arrange: Create a MemberDTO
        var testDate = new DateTime(1990, 1, 1);
        var dto = new MemberDTO
        {
            MemberID = 99,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = testDate,
            BranchId = 1,
            MembershipTypeId = 2,
            Address = "123 Main St",
            EmailAddress = "john@example.com"
        };

        // Act: Use your extension method
        var entity = dto.ToEntity();

        // Assert
        entity.MemberID.Should().Be(dto.MemberID);
        entity.FirstName.Should().Be("John");
        entity.LastName.Should().Be("Doe");
        // Check if DateOnly conversion worked
        entity.BirthDate.Should().Be(DateOnly.FromDateTime(testDate));
        entity.BranchId.Should().Be(dto.BranchId);
    }

    [Fact]
    public void ToDto_ShouldHandleNullNavigationProperties()
    {
        // Arrange: Provide a valid birthdate so the constructor doesn't throw.
        // Navigation properties (Branch, MembershipType) are null by default 
        // when using this constructor unless you manually assign them.
        var member = new Member(
            "Alex",
            "Vane",
            new DateOnly(1995, 1, 1), // BirthDate is now valid
            1,
            1,
            null,
            null,
            null
        );

        // Act
        var dto = member.ToDto();

        // Assert
        // Assuming your ToDto mapper uses "N/A" when member.Branch is null
        dto.Branch.Should().Be("N/A");
        dto.MembershipType.Should().Be("N/A");
    }
}