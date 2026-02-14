using MemberManagement.Domain.Entities;
using System;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Domain.Entities
{
    public class MemberTests
    {
        [Fact]
        public void Initialize_ShouldSetDefaultValues()
        {
            // Arrange
            var member = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                IsActive = false, // Set to false initially
                DateCreated = DateTime.MinValue // Set to empty date
            };

            // Act
            member.Initialize();

            // Assert
            Assert.True(member.IsActive, "Initialize should set IsActive to true.");

            // We check if the date is close to 'Now' (within 2 seconds) 
            // because UtcNow changes every millisecond.
            var timeDifference = DateTime.UtcNow - member.DateCreated;
            Assert.True(timeDifference.TotalSeconds < 2, "DateCreated should be set to the current UTC time.");
        }

        [Fact]
        public void Member_ShouldHoldDataCorrectly()
        {
            // Arrange
            var birthDate = new DateOnly(1995, 12, 25);
            var branch = new Branch("Main");

            // Act
            var member = new Member("Jane", "Smith", birthDate, 1, 1, "Address", "555", "email@test.com");
        }
    }
}