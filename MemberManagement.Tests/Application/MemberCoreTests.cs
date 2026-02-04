using FluentValidation;
using FluentValidation.Results;
using MemberManagement.Application.Business;
using MemberManagement.Application.Core;
using MemberManagement.Application.Interfaces;
using MemberManagement.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.Tests.Application
{
    public class MemberCoreTests
    {
        private MemberDTO CreateSampleDto() => new()
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateTime.Today,
            Address = "Addr",
            Branch = "B1",
            ContactNo = "123",
            EmailAddress = "john@test.com"
        };

        private Member CreateSampleMember(int id = 1) => new()
        {
            MemberID = id,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = DateOnly.FromDateTime(DateTime.Today),
            Address = "Addr",
            Branch = "B1",
            ContactNo = "123",
            EmailAddress = "john@test.com",
            IsActive = true,
            DateCreated = DateTime.Now
        };

        [Fact]
        public async Task CreateMemberAsync_ValidDto_CallsService()
        {
            var mockService = new Mock<IMemberService>();
            var mockValidator = new Mock<IValidator<Member>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<Member>())).Returns(new ValidationResult());

            var core = new MemberCore(mockService.Object, mockValidator.Object);

            var dto = CreateSampleDto();

            await core.CreateMemberAsync(dto);

            mockService.Verify(s => s.CreateAsync(It.IsAny<Member>()), Times.Once);
        }

        [Fact]
        public async Task CreateMemberAsync_InvalidDto_ThrowsValidationException()
        {
            var mockService = new Mock<IMemberService>();
            var mockValidator = new Mock<IValidator<Member>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<Member>()))
                         .Returns(new ValidationResult(new List<ValidationFailure> { new("FirstName", "Required") }));

            var core = new MemberCore(mockService.Object, mockValidator.Object);

            await Assert.ThrowsAsync<ValidationException>(() => core.CreateMemberAsync(CreateSampleDto()));
        }

        [Fact]
        public async Task UpdateMemberAsync_ValidDto_CallsService()
        {
            var member = CreateSampleMember();
            var mockService = new Mock<IMemberService>();
            mockService.Setup(s => s.GetByIdAsync(member.MemberID)).ReturnsAsync(member);
            mockService.Setup(s => s.UpdateAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);

            var mockValidator = new Mock<IValidator<Member>>();
            mockValidator.Setup(v => v.Validate(It.IsAny<Member>())).Returns(new ValidationResult());

            var core = new MemberCore(mockService.Object, mockValidator.Object);

            var dto = CreateSampleDto();
            dto.MemberID = member.MemberID;

            await core.UpdateMemberAsync(dto);

            mockService.Verify(s => s.UpdateAsync(It.IsAny<Member>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMemberAsync_NotFound_ThrowsKeyNotFoundException()
        {
            var mockService = new Mock<IMemberService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Member?)null);

            var mockValidator = new Mock<IValidator<Member>>();
            var core = new MemberCore(mockService.Object, mockValidator.Object);

            var dto = CreateSampleDto();
            dto.MemberID = 1;

            await Assert.ThrowsAsync<KeyNotFoundException>(() => core.UpdateMemberAsync(dto));
        }

        [Fact]
        public async Task DeleteMemberAsync_Valid_CallsService()
        {
            var member = CreateSampleMember();
            var mockService = new Mock<IMemberService>();
            mockService.Setup(s => s.GetByIdAsync(member.MemberID)).ReturnsAsync(member);
            mockService.Setup(s => s.UpdateAsync(member)).Returns(Task.CompletedTask);

            var core = new MemberCore(mockService.Object, new Mock<IValidator<Member>>().Object);

            await core.DeleteMemberAsync(member.MemberID);

            Assert.False(member.IsActive);
            mockService.Verify(s => s.UpdateAsync(member), Times.Once);
        }

        [Fact]
        public async Task DeleteMemberAsync_NotFound_ThrowsException()
        {
            var mockService = new Mock<IMemberService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Member?)null);

            var core = new MemberCore(mockService.Object, new Mock<IValidator<Member>>().Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => core.DeleteMemberAsync(1));
        }

        [Fact]
        public async Task GetMemberByIdAsync_Found_ReturnsDto()
        {
            var member = CreateSampleMember();
            var mockService = new Mock<IMemberService>();
            mockService.Setup(s => s.GetByIdAsync(member.MemberID)).ReturnsAsync(member);

            var core = new MemberCore(mockService.Object, new Mock<IValidator<Member>>().Object);

            var result = await core.GetMemberByIdAsync(member.MemberID);

            Assert.NotNull(result);
            Assert.Equal(member.MemberID, result!.MemberID);
        }

        [Fact]
        public async Task GetMemberByIdAsync_NotFound_ReturnsNull()
        {
            var mockService = new Mock<IMemberService>();
            mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Member?)null);

            var core = new MemberCore(mockService.Object, new Mock<IValidator<Member>>().Object);

            var result = await core.GetMemberByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetMembersForIndexAsync_FiltersAndSortsCorrectly()
        {
            var members = new List<Member>
        {
            CreateSampleMember(1),
            new Member
            {
                MemberID = 2,
                FirstName = "Alice",
                LastName = "Smith",
                Address = "Some Address",
                Branch = "B2",
                ContactNo = "555-2222",
                EmailAddress = "alice@test.com",
                BirthDate = DateOnly.FromDateTime(DateTime.Today),
                IsActive = true,
                DateCreated = DateTime.Now
            },
            new Member
            {
                MemberID = 3,
                FirstName = "Bob",
                LastName = "Doe",
                Address = "Another Addr",
                Branch = "B1",
                ContactNo = "555-3333",
                EmailAddress = "bob@test.com",
                BirthDate = DateOnly.FromDateTime(DateTime.Today),
                IsActive = true,
                DateCreated = DateTime.Now
            }
        };

            var mockService = new Mock<IMemberService>();
            mockService.Setup(s => s.GetActiveMembersAsync()).ReturnsAsync(members);

            var core = new MemberCore(mockService.Object, new Mock<IValidator<Member>>().Object);

            var result = await core.GetMembersForIndexAsync("Doe", "B1", "FirstName", "asc");

            Assert.Equal(2, result.Members.Count);
            Assert.Equal("Bob", result.Members.First().FirstName);
            Assert.Contains("B1", result.Branches);
        }
    }
}
