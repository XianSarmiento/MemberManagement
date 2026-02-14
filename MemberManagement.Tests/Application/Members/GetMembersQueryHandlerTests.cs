using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Members;
using MemberManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Application.Members
{
    public class GetMembersQueryHandlerTests
    {
        private readonly Mock<IMemberService> _mockService;
        private readonly GetMembersQueryHandler _handler;

        public GetMembersQueryHandlerTests()
        {
            _mockService = new Mock<IMemberService>();
            _handler = new GetMembersQueryHandler(_mockService.Object);
        }

        private List<Member> GetFakeMembers()
        {
            var branchNorth = new Branch("North");
            var branchSouth = new Branch("South");

            return new List<Member>
    {
        new Member("Alice", "Smith", new DateOnly(1990, 1, 1), 1, 1) { MemberID = 1 },
        new Member("Charlie", "Brown", new DateOnly(1990, 1, 1), 2, 1) { MemberID = 2 },
        new Member("Bob", "Smith", new DateOnly(1990, 1, 1), 1, 1) { MemberID = 3 }
    };
        }

        [Fact]
        public async Task HandleAsync_ShouldFilterByLastName()
        {
            // Arrange
            _mockService.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(GetFakeMembers());

            // Act
            var result = await _handler.HandleAsync("Smith", null!, "", "");

            // Assert
            Assert.Equal(2, result.TotalItems);
            Assert.All(result.Members, m => Assert.Equal("Smith", m.LastName));
        }

        [Fact]
        public async Task HandleAsync_ShouldFilterByBranch()
        {
            // Arrange
            _mockService.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(GetFakeMembers());

            // Act
            var result = await _handler.HandleAsync(null!, "South", "", "");

            // Assert
            Assert.Single(result.Members);
            Assert.Equal("South", result.Members[0].Branch);
        }

        [Fact]
        public async Task HandleAsync_ShouldSortByFirstNameDescending()
        {
            // Arrange
            _mockService.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(GetFakeMembers());

            // Act
            var result = await _handler.HandleAsync(null!, null!, "FirstName", "desc");

            // Assert
            Assert.Equal("Charlie", result.Members[0].FirstName);
            Assert.Equal("Bob", result.Members[1].FirstName);
            Assert.Equal("Alice", result.Members[2].FirstName);
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnUniqueSortedBranches()
        {
            // Arrange
            _mockService.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(GetFakeMembers());

            // Act
            var result = await _handler.HandleAsync(null!, null!, "", "");

            // Assert
            Assert.Equal(2, result.Branches.Count); 
            Assert.Equal("North", result.Branches[0]); 
            Assert.Equal("South", result.Branches[1]);
        }
    }
}