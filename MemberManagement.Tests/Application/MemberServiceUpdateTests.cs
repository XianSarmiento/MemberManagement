using MemberManagement.Application.Services;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MemberManagement.Tests.Application
{
    public class MemberServiceUpdateTests
    {
        [Fact]
        public async Task UpdateMember_Should_Call_Repository_UpdateAsync()
        {
            // Arrange
            var mockRepo = new Mock<IMemberRepository>();
            var service = new MemberService(mockRepo.Object);

            var member = new Member
            {
                MemberID = 1,
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = new DateOnly(2000, 1, 1),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "john@example.com"
            };

            // Act
            await service.UpdateAsync(member);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Member>(m => m.MemberID == 1)), Times.Once);
        }
    }
}
