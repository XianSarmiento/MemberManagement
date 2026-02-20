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

        [Theory]
        [InlineData("Regular Member", "R01", 50, "Member from Catanduanes, Albay, Sorsogon, Camarines Sur")]
        [InlineData("Associate Member", "A01", 50, "Member from Pio Duran, Polangui, Labay, Pili, Calabanga, Ragay, Caramoan, Sipocot")]
        [InlineData("Balik-Sagip Member", "B01", 100, "Special membership for returning members")]
        [InlineData("Extension Member", "E01", 200, "Extended membership plan")]
        public async Task Handle_ShouldUpdateAndSaveMembershipType(string name, string code, decimal fee, string desc)
        {
            // Arrange
            var existingType = new MembershipType("Old Name", "OLD01", 10m, "Old Desc");
            int targetId = 1;

            _repoMock.Setup(r => r.GetByIdAsync(targetId)).ReturnsAsync(existingType);

            // Act
            await _handler.Handle(targetId, name, code, fee, desc);

            // Assert
            existingType.Name.Should().Be(name);
            existingType.MembershipCode.Should().Be(code);
            existingType.MembershipFee.Should().Be(fee);
            existingType.Description.Should().Be(desc);

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