using FluentAssertions;
using MemberManagement.Application.Interfaces;
using MemberManagement.Application.Members;
using MemberManagement.Domain.Entities;
using Moq;
using Xunit;

namespace MemberManagement.Test.Members
{
    public class GetMembersQueryHandlerTests
    {
        private readonly Mock<IMemberService> _memberServiceMock;
        private readonly GetMembersQueryHandler _handler;

        public GetMembersQueryHandlerTests()
        {
            _memberServiceMock = new Mock<IMemberService>();
            _handler = new GetMembersQueryHandler(_memberServiceMock.Object);
        }

        private List<Member> GetSampleMembers()
        {
            // Fix: We provide values for the optional parameters (address, contact, email) 
            // to ensure the properties aren't uninitialized/null.
            return new List<Member>
            {
                new Member("Alice", "Zubiri", new DateOnly(1995, 1, 1), 1, 1, "", "", "") { MemberID = 10 },
                new Member("Charlie", "Brown", new DateOnly(1995, 1, 1), 1, 1, "", "", "") { MemberID = 20 },
                new Member("Bob", "Adams", new DateOnly(1995, 1, 1), 2, 1, "", "", "") { MemberID = 5 }
            };
        }

        [Fact]
        public async Task HandleAsync_ShouldFilterByLastName()
        {
            // Arrange
            var members = GetSampleMembers();
            _memberServiceMock.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(members);

            // Act: Search for "Brown" - Passing "" instead of null for required string params
            var result = await _handler.HandleAsync("Brown", "", "LastName", "asc");

            // Assert
            result.Members.Should().HaveCount(1);
            result.Members.First().LastName.Should().Be("Brown");
        }

        [Fact]
        public async Task HandleAsync_ShouldSortByFirstName_Descending()
        {
            // Arrange
            var members = GetSampleMembers();
            _memberServiceMock.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(members);

            // Act: Using empty strings instead of null literals
            var result = await _handler.HandleAsync("", "", "FirstName", "desc");

            // Assert
            result.Members.Should().HaveCount(3);
            result.Members.First().FirstName.Should().Be("Charlie");
            result.Members.Last().FirstName.Should().Be("Alice");
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnUniqueSortedBranches()
        {
            // Arrange
            var members = GetSampleMembers();
            _memberServiceMock.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(members);

            // Act
            var result = await _handler.HandleAsync("", "", "MemberID", "asc");

            // Assert
            result.Branches.Should().NotBeNull();
            result.Branches.Should().BeInAscendingOrder();
        }

        [Fact]
        public async Task HandleAsync_WhenSwitchingToInactive_ShouldCallGetInactiveMembers()
        {
            // Arrange
            _memberServiceMock.Setup(s => s.GetInactiveMembersAsync())
                .ReturnsAsync(new List<Member>());

            // Act
            await _handler.HandleAsync("", "", "MemberID", "asc", false);

            // Assert
            _memberServiceMock.Verify(s => s.GetInactiveMembersAsync(), Times.Once);
        }
    }
}