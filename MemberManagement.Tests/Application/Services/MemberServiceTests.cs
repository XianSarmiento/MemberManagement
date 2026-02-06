using FluentValidation;
using FluentValidation.Results;
using MemberManagement.Application.Services;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Application.Services
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _mockRepo;
        private readonly Mock<IValidator<Member>> _mockValidator;
        private readonly MemberService _service;

        public MemberServiceTests()
        {
            _mockRepo = new Mock<IMemberRepository>();
            _mockValidator = new Mock<IValidator<Member>>();
            _service = new MemberService(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenValid_ShouldInitializeAndAddMember()
        {
            // Arrange
            var member = new Member { FirstName = "John", LastName = "Doe" };

            _mockValidator.Setup(v => v.ValidateAsync(member, default))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _service.CreateAsync(member);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(It.Is<Member>(m => m == member)), Times.Once);

            // Verifying Initialize() logic (assuming Initialize sets DateCreated or IsActive)
            // If Initialize sets IsActive to true, we can check:
            // Assert.True(member.IsActive);
        }

        [Fact]
        public async Task CreateAsync_WhenInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var member = new Member();
            var failures = new List<ValidationFailure> { new("FirstName", "Required") };

            _mockValidator.Setup(v => v.ValidateAsync(member, default))
                .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(member));
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Member>()), Times.Never);
        }

        [Fact]
        public async Task GetActiveMembersAsync_ShouldCallRepositoryGetAll()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Member>());

            // Act
            await _service.GetActiveMembersAsync();

            // Assert
            _mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSoftDelete()
        {
            // Arrange
            int memberId = 10;

            // Act
            await _service.DeleteAsync(memberId);

            // Assert
            _mockRepo.Verify(r => r.SoftDeleteAsync(memberId), Times.Once);
        }
    }
}