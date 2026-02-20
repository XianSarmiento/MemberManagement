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
            var member = new Member(
                "John Christian",
                "Sarmiento",
                new DateOnly(1997, 8, 7),
                branchId: 1,
                membershipTypeId: 1 // Regular
            )
            {
                IsActive = false
            };

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
            var member = new Member(
                "John Christian",
                "Sarmiento",
                new DateOnly(1997, 8, 7),
                branchId: 1,
                membershipTypeId: 1 // Regular
            );

            _validatorMock.Setup(v => v.ValidateAsync(member, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _service.CreateAsync(member);

            // Assert
            member.IsActive.Should().BeTrue();
            _repoMock.Verify(r => r.AddAsync(member), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenInvalid_ShouldThrowValidationException()
        {
            // Arrange
            var member = new Member(
                "John Christian",
                "Sarmiento",
                new DateOnly(1997, 8, 7),
                branchId: 1,
                membershipTypeId: 1 // Regular
            );

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
            var m1 = new Member(
                "John Christian",
                "Sarmiento",
                new DateOnly(1997, 8, 7),
                branchId: 1,
                membershipTypeId: 1 // Regular
            )
            {
                IsActive = true
            };

            var m2 = new Member(
                "Ana",
                "Lopez",
                new DateOnly(1998, 2, 5),
                branchId: 2,
                membershipTypeId: 4 // Extension
            )
            {
                IsActive = false
            };

            _repoMock.Setup(r => r.GetAllAsync(false)).ReturnsAsync(new List<Member> { m1, m2 });

            // Act
            var result = await _service.GetInactiveMembersAsync();

            // Assert
            var inactiveList = result.ToList();
            inactiveList.Should().HaveCount(1);
            inactiveList.First().FirstName.Should().Be("Ana");
        }
    }
}