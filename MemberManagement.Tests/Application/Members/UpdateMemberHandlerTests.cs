using FluentValidation;
using FluentValidation.Results;
using MemberManagement.SharedKernel.Constant;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Members;
using MemberManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Application.Members
{
    public class UpdateMemberHandlerTests
    {
        private readonly Mock<IMemberService> _mockService;
        private readonly Mock<IValidator<Member>> _mockValidator;
        private readonly UpdateMemberHandler _handler;

        public UpdateMemberHandlerTests()
        {
            _mockService = new Mock<IMemberService>();
            _mockValidator = new Mock<IValidator<Member>>();
            _handler = new UpdateMemberHandler(_mockService.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenMemberExistsAndValid_ShouldUpdateMember()
        {
            // Arrange
            var dto = new MemberDTO
            {
                MemberID = 1,
                FirstName = "UpdatedName",
                LastName = "UpdatedLast"
            };

            var existingMember = new Member { MemberID = 1, FirstName = "OldName", LastName = "OldLast" };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingMember);

            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Member>(), default))
                          .ReturnsAsync(new ValidationResult());

            // Act
            await _handler.HandleAsync(dto);

            // Assert
            Assert.Equal("UpdatedName", existingMember.FirstName);
            _mockService.Verify(s => s.UpdateAsync(It.IsAny<Member>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WhenMemberNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var dto = new MemberDTO { MemberID = 99 };
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Member?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.HandleAsync(dto));
            Assert.Equal(OperationMessage.Error.NotFound, exception.Message);

            _mockService.Verify(s => s.UpdateAsync(It.IsAny<Member>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var dto = new MemberDTO { MemberID = 1, FirstName = "" };
            var existingMember = new Member { MemberID = 1 };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingMember);

            var failures = new List<ValidationFailure> { new("FirstName", "Required") };
            _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Member>(), default))
                          .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.HandleAsync(dto));
            _mockService.Verify(s => s.UpdateAsync(It.IsAny<Member>()), Times.Never);
        }
    }
}