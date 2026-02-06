using MemberManagement.Application.DTOs;
using System;
using Xunit;
using Assert = Xunit.Assert;



namespace MemberManagement.UnitTests.Application.DTOs
{
    public class MemberDTOTests
    {
        [Fact]
        public void MemberDTO_ShouldSetAndGetPropertiesCorrectly()
        {
            // Arrange
            var testDate = new DateTime(1990, 1, 1);
            var createdDate = DateTime.Now;

            // Act
            var member = new MemberDTO
            {
                MemberID = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = testDate,
                Address = "123 Main St",
                Branch = "Central",
                ContactNo = "09123456789",
                EmailAddress = "john.doe@example.com",
                IsActive = true,
                DateCreated = createdDate
            };

            // Assert
            Assert.Equal(1, member.MemberID);
            Assert.Equal("John", member.FirstName);
            Assert.Equal("Doe", member.LastName);
            Assert.Equal(testDate, member.BirthDate);
            Assert.Equal("123 Main St", member.Address);
            Assert.Equal("Central", member.Branch);
            Assert.Equal("09123456789", member.ContactNo);
            Assert.Equal("john.doe@example.com", member.EmailAddress);
            Assert.True(member.IsActive);
            Assert.Equal(createdDate, member.DateCreated);
        }

        [Fact]
        public void MemberDTO_Nullables_ShouldAllowNulls()
        {
            // Act
            var member = new MemberDTO
            {
                FirstName = null,
                LastName = null,
                EmailAddress = null
            };

            // Assert
            Assert.Null(member.FirstName);
            Assert.Null(member.LastName);
            Assert.Null(member.EmailAddress);
        }
    }
}