using FluentValidation;
using MemberManagement.Application.Services;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using FluentValidation.Results;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MemberManagement.Tests.Application
{
    public class MemberServiceDeleteTests
    {
        [Fact]
        public async Task DeleteMember_Should_Call_Repository_DeleteAsync()
        {
            // Arrange
            var mockRepo = new Mock<IMemberRepository>();
            var mockValidator = new Mock<IValidator<Member>>();
            mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Member>(), default))
                         .ReturnsAsync(new ValidationResult()); // always valid

            var service = new MemberService(mockRepo.Object, mockValidator.Object);

            var memberId = 1;

            // Act
            await service.DeleteAsync(memberId);

            // Assert
            mockRepo.Verify(r => r.SoftDeleteAsync(memberId), Times.Once);
        }
    }
}
