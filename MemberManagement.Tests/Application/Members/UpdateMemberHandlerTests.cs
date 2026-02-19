using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Members;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using MemberManagement.SharedKernel.Constant;
using Moq;
using Xunit;

namespace MemberManagement.Test.Members
{
    public class UpdateMemberHandlerTests
    {
        private readonly Mock<IMemberService> _memberServiceMock;
        private readonly Mock<IValidator<Member>> _validatorMock;
        private readonly UpdateMemberHandler _handler;

        public UpdateMemberHandlerTests()
        {
            _memberServiceMock = new Mock<IMemberService>();
            _validatorMock = new Mock<IValidator<Member>>();
            _handler = new UpdateMemberHandler(_memberServiceMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenMemberExists_ShouldUpdateFieldsAndCallService()
        {
            // Arrange
            // Create existing member (Year 1995 to pass age validation)
            var existingMember = new Member("OldName", "OldLastName", new DateOnly(1995, 1, 1), 1, 1, "", "", "");

            var dto = new MemberDTO
            {
                MemberID = 1,
                FirstName = "NewName",
                LastName = "NewLastName",
                BirthDate = new DateTime(1995, 1, 1),
                BranchId = 2,
                Address = "New Address",
                ContactNo = "123",
                EmailAddress = "test@test.com"
            };

            _memberServiceMock.Setup(s => s.GetByIdAsync(dto.MemberID)).ReturnsAsync(existingMember);
            _validatorMock.Setup(v => v.ValidateAsync(existingMember, default)).ReturnsAsync(new ValidationResult());

            // Act
            await _handler.HandleAsync(dto);

            // Assert
            existingMember.FirstName.Should().Be("NewName");
            existingMember.BranchId.Should().Be(2);
            _memberServiceMock.Verify(s => s.UpdateAsync(existingMember), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WhenMemberNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var dto = new MemberDTO { MemberID = 99, FirstName = "", LastName = "" };
            _memberServiceMock.Setup(s => s.GetByIdAsync(dto.MemberID)).ReturnsAsync((Member?)null);

            // Act & Assert
            var act = () => _handler.HandleAsync(dto);
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage(OperationMessage.Error.NotFound);
        }

        [Fact]
        public async Task HandleAsync_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var existingMember = new Member("John", "Doe", new DateOnly(1995, 1, 1), 1, 1, "", "", "");
            var dto = new MemberDTO { MemberID = 1, FirstName = "John", LastName = "Doe" };

            var failures = new List<ValidationFailure> { new("FirstName", "Invalid") };

            _memberServiceMock.Setup(s => s.GetByIdAsync(dto.MemberID)).ReturnsAsync(existingMember);
            _validatorMock.Setup(v => v.ValidateAsync(existingMember, default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<ValidationException>(() => _handler.HandleAsync(dto));
            _memberServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Member>()), Times.Never);
        }
    }
}