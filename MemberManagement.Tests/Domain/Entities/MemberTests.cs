using MemberManagement.Domain.Entities;
using MemberManagement.SharedKernel.Constant;
using FluentAssertions;
using Xunit;
using System;

namespace MemberManagement.UnitTests.Domain.Entities
{
    public class MemberTests
    {
        private readonly DateOnly validBirthDate = new DateOnly(1997, 8, 7);

        [Fact]
        public void Constructor_Should_Set_Branch_And_MembershipType()
        {
            var member = new Member("John Christian", "Sarmiento", validBirthDate, 10, 20);

            member.BranchId.Should().Be(10);
            member.MembershipTypeId.Should().Be(20);
        }

        [Fact]
        public void ChangeBranch_Should_Update_BranchId()
        {
            var member = new Member("John Christian", "Sarmiento", validBirthDate, 1, 1);

            member.ChangeBranch(5);

            member.BranchId.Should().Be(5);
        }

        [Fact]
        public void Constructor_WithValidData_ShouldInitializeCorrectly()
        {
            var member = new Member(
                "John Christian",
                "Sarmiento",
                validBirthDate,
                1,
                1,
                "Manila",
                "09123456789",
                "john.sarmiento@test.com"
            );

            member.FirstName.Should().Be("John Christian");
            member.LastName.Should().Be("Sarmiento");
            member.IsActive.Should().BeTrue();
            member.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Constructor_WhenBirthDateNull_ShouldThrowException()
        {
            Action act = () => new Member("John Christian", "Sarmiento", null!, 1, 1);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage(OperationMessage.Error.BirthDateRequired);
        }

        [Theory]
        [InlineData(-17)]
        [InlineData(-5)]
        public void Constructor_WhenUnderage_ShouldThrowException(int yearsToAdd)
        {
            var birthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(yearsToAdd);

            Action act = () => new Member("John Christian", "Sarmiento", birthDate, 1, 1);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage(OperationMessage.Error.Underage);
        }

        [Fact]
        public void Constructor_WhenExceedsAgeLimit_ShouldThrowException()
        {
            var birthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-70);

            Action act = () => new Member("John Christian", "Sarmiento", birthDate, 1, 1);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage(OperationMessage.Error.ExceedsAgeLimit);
        }

        [Fact]
        public void Constructor_WhenExactly18YearsOld_ShouldSucceed()
        {
            var birthDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-18);

            var member = new Member("John Christian", "Sarmiento", birthDate, 1, 1);

            member.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_ShouldResetState()
        {
            var member = new Member();

            member.Initialize();

            member.IsActive.Should().BeTrue();
            member.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Constructor_Should_Preserve_All_Provided_Data()
        {
            var member = new Member("John Christian", "Sarmiento", validBirthDate, 3, 7);

            member.FirstName.Should().Be("John Christian");
            member.LastName.Should().Be("Sarmiento");
            member.BranchId.Should().Be(3);
            member.MembershipTypeId.Should().Be(7);
        }
    }
}