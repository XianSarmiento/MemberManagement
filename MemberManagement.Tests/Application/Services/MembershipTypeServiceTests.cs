using FluentAssertions;
using MemberManagement.Application.Services;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace MemberManagement.Test.Services
{
    public class MembershipTypeServiceTests
    {
        private readonly Mock<IMembershipTypeRepository> _repoMock;
        private readonly MembershipTypeService _service;

        public MembershipTypeServiceTests()
        {
            _repoMock = new Mock<IMembershipTypeRepository>();
            _service = new MembershipTypeService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMembershipTypes()
        {
            // Arrange
            // Fix: Use the PUBLIC constructor: MembershipType(name, code, fee, description)
            var types = new List<MembershipType>
            {
                new MembershipType("Regular", "REG01", 1000m, "Standard membership"),
                new MembershipType("Premium", "PREM01", 5000m, "Elite membership")
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(types);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(t => t.MembershipCode == "REG01");
            result.Should().Contain(t => t.MembershipCode == "PREM01");

            // Verify the repository was actually called
            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoData_ShouldReturnEmptyList()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<MembershipType>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}