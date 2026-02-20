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

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<IValidationContext>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<MembershipType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _handler = new CreateMembershipTypeHandler(_repoMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateRegularMember()
        {
            string name = "Regular Member";
            string code = "R01";
            decimal fee = 50m;
            string desc = "Member from Catanduanes, Albay, Sorsogon, Camarines Sur";

            var resultId = await _handler.Handle(name, code, fee, desc);

            _repoMock.Verify(r => r.AddAsync(It.Is<MembershipType>(m =>
                m.Name == name &&
                m.MembershipCode == code &&
                m.MembershipFee == fee &&
                m.Description == desc &&
                m.IsActive == true
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateAssociateMember()
        {
            string name = "Associate Member";
            string code = "A01";
            decimal fee = 50m;
            string desc = "Member from Pio Duran, Polangui, Labay, Pili, Calabanga, Ragay, Caramoan, Sipocot";

            var resultId = await _handler.Handle(name, code, fee, desc);

            _repoMock.Verify(r => r.AddAsync(It.Is<MembershipType>(m =>
                m.Name == name &&
                m.MembershipCode == code &&
                m.MembershipFee == fee &&
                m.Description == desc &&
                m.IsActive == true
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateBalikSagipMember()
        {
            string name = "Balik-Sagip Member";
            string code = "B01";
            decimal fee = 100m;
            string desc = "Special membership for returning members";

            var resultId = await _handler.Handle(name, code, fee, desc);

            _repoMock.Verify(r => r.AddAsync(It.Is<MembershipType>(m =>
                m.Name == name &&
                m.MembershipCode == code &&
                m.MembershipFee == fee &&
                m.Description == desc &&
                m.IsActive == true
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateExtensionMember()
        {
            string name = "Extension Member";
            string code = "E01";
            decimal fee = 200m;
            string desc = "Extended membership plan";

            var resultId = await _handler.Handle(name, code, fee, desc);

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
            var result = await _handler.Handle("Sample Member", "S01", 500m, "Basic");

            result.Should().Be(0);
        }
    }
}