using FluentValidation;
using FluentValidation.Results;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Members;
using MemberManagement.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;



namespace MemberManagement.UnitTests.Application.Members
{
    public class CreateMemberHandlerTests
    {
        private readonly Mock<IMemberService> _mockService;
        private readonly Mock<IValidator<Member>> _mockValidator;
        private readonly CreateMemberHandler _handler;

        public CreateMemberHandlerTests()
        {
            _mockService = new Mock<IMemberService>();
            _mockValidator = new Mock<IValidator<Member>>();
            _handler = new CreateMemberHandler(_mockService.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenValidationSucceeds_ShouldCallCreateAsync()
        {
            // Arrange
            var dto = new MemberDTO { FirstName = "John", LastName = "Doe" };

            // Mock validator to return Success
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Member>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _handler.HandleAsync(dto);

            // Assert
            _mockService.Verify(s => s.CreateAsync(It.IsAny<Member>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var dto = new MemberDTO { FirstName = "" }; // Invalid data
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("FirstName", "First Name is required")
            };

            // Mock validator to return failure
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Member>(), default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.HandleAsync(dto));

            // Verify that the service was NEVER called because of the exception
            _mockService.Verify(s => s.CreateAsync(It.IsAny<Member>()), Times.Never);
        }
    }
}