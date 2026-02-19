using FluentAssertions;
using MemberManagement.Application.MembershipTypes;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace MemberManagement.Test.MembershipTypes
{
    public class UpdateMembershipTypeHandlerTests
    {
        private readonly Mock<IMembershipTypeRepository> _repoMock;
        private readonly Mock<IValidator<MembershipType>> _validatorMock;
        private readonly UpdateMembershipTypeHandler _handler;

        public UpdateMembershipTypeHandlerTests()
        {
            _repoMock = new Mock<IMembershipTypeRepository>();
            _validatorMock = new Mock<IValidator<MembershipType>>();

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<MembershipType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<MembershipType>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _handler = new UpdateMembershipTypeHandler(_repoMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_WhenEntityExists_ShouldUpdateAndSave()
        {
            // Arrange
            var existingType = new MembershipType("Old Name", "OLD01", 100m, "Old Desc");
            int targetId = 1;

            _repoMock.Setup(r => r.GetByIdAsync(targetId)).ReturnsAsync(existingType);

            string newName = "New Name";
            string newCode = "NEW01";
            decimal newFee = 200m;
            string newDesc = "New Desc";

            // Act
            await _handler.Handle(targetId, newName, newCode, newFee, newDesc);

            // Assert
            existingType.Name.Should().Be(newName);
            existingType.MembershipFee.Should().Be(newFee);
            _repoMock.Verify(r => r.UpdateAsync(existingType), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenEntityDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((MembershipType?)null);

            // Act & Assert
            var act = () => _handler.Handle(99, "Name", "Code", 100m, "Desc");

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Membership type not found.");

            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<MembershipType>()), Times.Never);
        }
    }
}