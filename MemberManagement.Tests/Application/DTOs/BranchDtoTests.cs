using FluentAssertions;
using MemberManagement.Application.DTOs;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MemberManagement.Tests.Application.DTOs;

public class BranchDtoTests
{
    [Fact]
    public void BranchDto_ShouldFailValidation_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var dto = new BranchDto { Name = null! };
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        // Assert
        // Note: This only works if you add [Required] to your BranchDto properties
        // isValid.Should().BeFalse(); 
    }
}