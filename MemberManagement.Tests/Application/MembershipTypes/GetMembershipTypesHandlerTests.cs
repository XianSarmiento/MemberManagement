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
            // Arrange
            // Using the public gatekeeper constructor from your entity
            var mockEntities = new List<MembershipType>
            {
                new MembershipType("Student", "STU01", 500m, "Student Discount Tier"),
                new MembershipType("Regular", "REG01", 1000m, "Standard Membership")
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(mockEntities);

            // Act
            var result = await _handler.Handle();

            // Assert
            var resultList = result.ToList();
            resultList.Should().HaveCount(2);

            // Check specific mapping values
            resultList.Should().ContainSingle(d => d.MembershipCode == "STU01" && d.Name == "Student");
            resultList.Should().ContainSingle(d => d.MembershipCode == "REG01" && d.MembershipFee == 1000m);

            // Verify all items are active (since constructor sets IsActive = true)
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