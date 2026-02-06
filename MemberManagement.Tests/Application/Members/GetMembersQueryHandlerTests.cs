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
            return new List<Member>
            {
                new Member { MemberID = 1, FirstName = "Alice", LastName = "Smith", Branch = "North", IsActive = true },
                new Member { MemberID = 2, FirstName = "Charlie", LastName = "Brown", Branch = "South", IsActive = true },
                new Member { MemberID = 3, FirstName = "Bob", LastName = "Smith", Branch = "North", IsActive = true }
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
            // Order should be: Charlie, Bob, Alice
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
            Assert.Equal(2, result.Branches.Count); // North and South
            Assert.Equal("North", result.Branches[0]); // Alphabetical
            Assert.Equal("South", result.Branches[1]);
        }
    }
}