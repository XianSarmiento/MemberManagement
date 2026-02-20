using FluentAssertions;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.MembershipTypes;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace MemberManagement.Test.MembershipTypes
{
    public class GetMembershipTypesHandlerTests
    {
        private readonly Mock<IMembershipTypeRepository> _repoMock;
        private readonly GetMembershipTypesHandler _handler;

        public GetMembershipTypesHandlerTests()
        {
            _repoMock = new Mock<IMembershipTypeRepository>();
            _handler = new GetMembershipTypesHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldMapEntitiesToDtosCorrectly()
        {
            // Arrange: Using your real membership types
            var mockEntities = new List<MembershipType>
            {
                new MembershipType("Regular Member", "R01", 50m, "Member from Catanduanes, Albay, Sorsogon, Camarines Sur"),
                new MembershipType("Associate Member", "A01", 50m, "Member from Pio Duran, Polangui, Labay, Pili, Calabanga, Ragay, Caramoan, Sipocot"),
                new MembershipType("Balik-Sagip Member", "B01", 100m, "Special membership for returning members"),
                new MembershipType("Extension Member", "E01", 200m, "Extended membership plan")
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(mockEntities);

            // Act
            var result = await _handler.Handle();

            // Assert
            var resultList = result.ToList();
            resultList.Should().HaveCount(4);

            resultList.Should().ContainSingle(d => d.MembershipCode == "R01" && d.Name == "Regular Member");
            resultList.Should().ContainSingle(d => d.MembershipCode == "A01" && d.Name == "Associate Member");
            resultList.Should().ContainSingle(d => d.MembershipCode == "B01" && d.Name == "Balik-Sagip Member");
            resultList.Should().ContainSingle(d => d.MembershipCode == "E01" && d.Name == "Extension Member");

            // Verify all items are active
            resultList.Should().AllSatisfy(d => d.IsActive.Should().BeTrue());

            _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoDataFound_ShouldReturnEmptyList()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<MembershipType>());

            // Act
            var result = await _handler.Handle();

            // Assert
            result.Should().BeEmpty();
        }
    }
}