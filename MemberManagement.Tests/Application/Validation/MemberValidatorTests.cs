using FluentValidation.TestHelper;
using MemberManagement.Application.Validation;
using MemberManagement.Domain.Entities;
using System;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Application.Validation
{
    public class MemberValidatorTests
    {
        private readonly MemberValidator _validator;

        public MemberValidatorTests()
        {
            _validator = new MemberValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Names_Are_Empty()
        {
            // Arrange
            var member = new Member { FirstName = "", LastName = "" };

            // Act & Assert
            var result = _validator.TestValidate(member);
            result.ShouldHaveValidationErrorFor(m => m.FirstName);
            result.ShouldHaveValidationErrorFor(m => m.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_BirthDate_Is_In_Future()
        {
            // Arrange
            var member = new Member
            {
                BirthDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            // Act & Assert
            var result = _validator.TestValidate(member);
            result.ShouldHaveValidationErrorFor(m => m.BirthDate)
                  .WithErrorMessage("BirthDate cannot be in the future.");
        }

        [Theory]
        [InlineData("12345")]          // Too short
        [InlineData("08123456789")]    // Doesn't start with 09
        [InlineData("0912345678A")]    // Contains letters
        public void Should_Have_Error_When_ContactNo_Is_Invalid(string invalidContact)
        {
            // Arrange
            var member = new Member { ContactNo = invalidContact };

            // Act & Assert
            var result = _validator.TestValidate(member);
            result.ShouldHaveValidationErrorFor(m => m.ContactNo)
                  .WithErrorMessage("Invalid PH number.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Email_Is_Valid()
        {
            // Arrange
            var member = new Member { EmailAddress = "test@example.com" };

            // Act & Assert
            var result = _validator.TestValidate(member);
            result.ShouldNotHaveValidationErrorFor(m => m.EmailAddress);
        }

        [Fact]
        public void Should_Be_Valid_When_All_Fields_Are_Correct()
        {
            // Arrange
            var member = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateOnly(1990, 1, 1),
                ContactNo = "09123456789",
                EmailAddress = "john@doe.com"
            };

            // Act
            var result = _validator.TestValidate(member);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}