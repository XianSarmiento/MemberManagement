using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Application.Interfaces
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberService> _mockMemberService;

        public MemberServiceTests()
        {
            _mockMemberService = new Mock<IMemberService>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMember_WhenMemberExists()
        {
            // Arrange
            var memberId = 1;
            var expectedMember = new Member { MemberID = memberId, FirstName = "John" };

            _mockMemberService
                .Setup(s => s.GetByIdAsync(memberId))
                .ReturnsAsync(expectedMember);

            var service = _mockMemberService.Object;

            // Act
            var result = await service.GetByIdAsync(memberId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memberId, result.MemberID);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetActiveMembersAsync_ShouldReturnOnlyActiveMembers()
        {
            // Arrange
            var activeMembers = new List<Member>
            {
                new Member { MemberID = 1, IsActive = true },
                new Member { MemberID = 2, IsActive = true }
            };

            _mockMemberService
                .Setup(s => s.GetActiveMembersAsync())
                .ReturnsAsync(activeMembers);

            var service = _mockMemberService.Object;

            // Act
            var result = await service.GetActiveMembersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.All(result, m => Assert.True(m.IsActive));
        }

        [Fact]
        public async Task CreateAsync_ShouldBeCalledOnce()
        {
            // Arrange
            var newMember = new Member { FirstName = "New", LastName = "Member" };
            var service = _mockMemberService.Object;

            // Act
            await service.CreateAsync(newMember);

            // Assert
            _mockMemberService.Verify(s => s.CreateAsync(newMember), Times.Once);
        }
    }
}