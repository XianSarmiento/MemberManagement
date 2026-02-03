using MemberManagement.Application.Services;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.Tests.Application
{
    public class MemberServiceGetTests
    {
        [Fact]
        public async Task GetMemberById_Should_Return_Member()
        {
            // Arrange
            var mockRepo = new Mock<IMemberRepository>();
            var member = new Member
            {
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = new DateOnly(2000, 1, 1),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "john@example.com"
            };
            mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(member);

            var service = new MemberService(mockRepo.Object);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Sarmiento", result.LastName);
        }
    }
}
