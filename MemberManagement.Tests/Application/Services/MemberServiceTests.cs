using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MemberManagement.Application.Services;
using MemberManagement.Domain.Entities;
using MemberManagement.Domain.Interfaces;
using Moq;
using Xunit;

namespace MemberManagement.Test.Services
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _repoMock;
        private readonly Mock<IValidator<Member>> _validatorMock;
        private readonly MemberService _service;

        public MemberServiceTests()
        {
            _repoMock = new Mock<IMemberRepository>();
            _validatorMock = new Mock<IValidator<Member>>();
            _service = new MemberService(_repoMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task RestoreAsync_WhenMemberExists_ShouldSetIsActiveAndSave()
        {
            // Arrange
            // 1995 is a "safe" year that passes your 18-65 age validation in the constructor
            var member = new Member("John", "Doe", new DateOnly(1995, 1, 1), 1, 1);

            // Directly set property since we aren't touching the entity class
            member.IsActive = false;

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(member);

            // Act
            await _service.RestoreAsync(1);

            // Assert
            member.IsActive.Should().BeTrue();
            _repoMock.Verify(r => r.UpdateAsync(member), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenValid_ShouldInitializeAndAdd()
        {
            // Arrange
            var member = new Member("Jane", "Doe", new DateOnly(1990, 5, 15), 1, 1);
            _validatorMock.Setup(v => v.ValidateAsync(member, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _service.CreateAsync(member);

            // Assert
            // Verifies that member.Initialize() was called inside the service
            member.IsActive.Should().BeTrue();
            _repoMock.Verify(r => r.AddAsync(member), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var member = new Member("John", "Doe", new DateOnly(1995, 1, 1), 1, 1);
            var failures = new List<ValidationFailure> { new("FirstName", "Invalid name") };

            _validatorMock.Setup(v => v.ValidateAsync(member, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(member));
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Member>()), Times.Never);
        }

        [Fact]
        public async Task GetInactiveMembersAsync_ShouldOnlyReturnInactiveMembers()
        {
            // Arrange
            var m1 = new Member("Active", "User", new DateOnly(1990, 1, 1), 1, 1);
            var m2 = new Member("Inactive", "User", new DateOnly(1990, 1, 1), 1, 1);

            m1.IsActive = true;
            m2.IsActive = false;

            _repoMock.Setup(r => r.GetAllAsync(false)).ReturnsAsync(new List<Member> { m1, m2 });

            // Act
            var result = await _service.GetInactiveMembersAsync();

            // Assert
            var inactiveList = result.ToList();
            inactiveList.Should().HaveCount(1);
            inactiveList.First().FirstName.Should().Be("Inactive");
        }
    }
}