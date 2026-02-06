using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Domain.Interfaces
{
    public class MemberRepositoryContractTests
    {
        private readonly Mock<IMemberRepository> _mockRepo;

        public MemberRepositoryContractTests()
        {
            _mockRepo = new Mock<IMemberRepository>();
        }

        [Fact]
        public async Task GetActiveAsync_ShouldReturnOnlyActiveMembers_WhenMocked()
        {
            // Arrange
            var activeMembers = new List<Member>
            {
                new Member { MemberID = 1, IsActive = true, FirstName = "Test" }
            };

            _mockRepo.Setup(repo => repo.GetActiveAsync())
                     .ReturnsAsync(activeMembers);

            // Act
            var result = await _mockRepo.Object.GetActiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            _mockRepo.Verify(repo => repo.GetActiveAsync(), Times.Once);
        }

        [Fact]
        public async Task SoftDeleteAsync_ShouldVerifyCallWithCorrectId()
        {
            // Arrange
            int targetId = 5;

            // Act
            await _mockRepo.Object.SoftDeleteAsync(targetId);

            // Assert
            _mockRepo.Verify(repo => repo.SoftDeleteAsync(targetId), Times.Once);
        }
    }
}