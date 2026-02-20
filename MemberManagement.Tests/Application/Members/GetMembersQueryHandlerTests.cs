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
            // Updated to use your real information
            return new List<Member>
            {
                new Member("John Christian", "Sarmiento", new DateOnly(1997, 8, 7), 1, 1, "Naga City", "09123456789", "john.sarmiento@email.com") { MemberID = 10 },
                new Member("Alice", "Zubiri", new DateOnly(1995, 1, 1), 1, 1, "", "", "") { MemberID = 20 },
                new Member("Bob", "Adams", new DateOnly(1995, 1, 1), 2, 1, "", "", "") { MemberID = 5 }
            };
        }

        [Fact]
        public async Task HandleAsync_ShouldFilterByLastName()
        {
            var members = GetSampleMembers();
            _memberServiceMock.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(members);

            var result = await _handler.HandleAsync("Sarmiento", "", "LastName", "asc");

            result.Members.Should().HaveCount(1);
            result.Members.First().LastName.Should().Be("Sarmiento");
        }

        [Fact]
        public async Task HandleAsync_ShouldSortByFirstName_Descending()
        {
            var members = GetSampleMembers();
            _memberServiceMock.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(members);

            var result = await _handler.HandleAsync("", "", "FirstName", "desc");

            result.Members.Should().HaveCount(3);
            result.Members.First().FirstName.Should().Be("John Christian");
            result.Members.Last().FirstName.Should().Be("Alice");
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnUniqueSortedBranches()
        {
            var members = GetSampleMembers();
            _memberServiceMock.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(members);

            var result = await _handler.HandleAsync("", "", "MemberID", "asc");

            result.Branches.Should().NotBeNull();
            result.Branches.Should().BeInAscendingOrder();
        }

        [Fact]
        public async Task HandleAsync_WhenSwitchingToInactive_ShouldCallGetInactiveMembers()
        {
            _memberServiceMock.Setup(s => s.GetInactiveMembersAsync())
                .ReturnsAsync(new List<Member>());

            await _handler.HandleAsync("", "", "MemberID", "asc", false);

            _memberServiceMock.Verify(s => s.GetInactiveMembersAsync(), Times.Once);
        }
    }
}