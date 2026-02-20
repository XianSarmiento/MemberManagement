using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Members;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using Moq;
using Xunit;

namespace MemberManagement.Test.Members
{
    public class CreateMemberHandlerTests
    {
        private readonly Mock<IMemberService> _memberServiceMock;
        private readonly Mock<IValidator<Member>> _validatorMock;
        private readonly CreateMemberHandler _handler;

        public CreateMemberHandlerTests()
        {
            _memberServiceMock = new Mock<IMemberService>();
            _validatorMock = new Mock<IValidator<Member>>();
            _handler = new CreateMemberHandler(_memberServiceMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenValidDto_ShouldCallCreateAsync()
        {
            // Arrange
            var dto = new MemberDTO
            {
                FirstName = "John Christian",
                LastName = "Sarmiento",
                BirthDate = new DateTime(1997, 8, 7),
                BranchId = 1,
                MembershipTypeId = 1
            };

            // Setup validator to return success
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Member>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _handler.HandleAsync(dto);

            // Assert
            _memberServiceMock.Verify(s => s.CreateAsync(It.IsAny<Member>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WhenInvalidDto_ShouldThrowValidationException()
        {
            // Arrange
            var dto = new MemberDTO
            {
                FirstName = "", // This makes it "Invalid" for the Validator
                LastName = "Sarmiento",
                BirthDate = new DateTime(1997, 8, 7), // This satisfies the Constructor Gatekeeper
                BranchId = 1,
                MembershipTypeId = 1
            };

            var failures = new List<ValidationFailure> { new("FirstName", "Required") };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<Member>(), default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<ValidationException>(() => _handler.HandleAsync(dto));

            // Verify the service was NEVER called because validation failed
            _memberServiceMock.Verify(s => s.CreateAsync(It.IsAny<Member>()), Times.Never);
        }
    }
}