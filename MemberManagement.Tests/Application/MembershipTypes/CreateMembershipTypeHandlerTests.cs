using FluentAssertions;
using FluentValidation;
using FluentValidation.Results; // Added for ValidationResult
using MemberManagement.Application.MembershipTypes;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace MemberManagement.Test.MembershipTypes
{
    public class CreateMembershipTypeHandlerTests
    {
        private readonly Mock<IMembershipTypeRepository> _repoMock;
        private readonly Mock<IValidator<MembershipType>> _validatorMock;
        private readonly CreateMembershipTypeHandler _handler;

        public CreateMembershipTypeHandlerTests()
        {
            _repoMock = new Mock<IMembershipTypeRepository>();
            _validatorMock = new Mock<IValidator<MembershipType>>();

            // FIX: This setup covers ALL overloads of ValidateAsync
            // including if your handler uses a ValidationContext internally.
            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Keeping this one as well just in case
            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<MembershipType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _handler = new CreateMembershipTypeHandler(_repoMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateEntityAndCallRepository()
        {
            // Arrange
            string name = "Gold";
            string code = "G01";
            decimal fee = 1500.00m;
            string desc = "Premium tier";

            // Act
            var resultId = await _handler.Handle(name, code, fee, desc);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<MembershipType>(m =>
                m.Name == name &&
                m.MembershipCode == code &&
                m.MembershipFee == fee &&
                m.Description == desc &&
                m.IsActive == true
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnIdFromCreatedEntity()
        {
            // Act
            var result = await _handler.Handle("Silver", "S01", 500m, "Basic");

            // Assert
            result.Should().Be(0);
        }
    }
}