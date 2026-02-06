using MemberManagement.Application.DTOs;
using MemberManagement.Application.Mappers;
using MemberManagement.Domain.Entities;
using System;
using Xunit;
using Assert = Xunit.Assert;



namespace MemberManagement.UnitTests.Application.Mappers
{
    public class MemberEntityMapperTests
    {
        [Fact]
        public void ToDto_ShouldMapEntityToDtoCorrectly()
        {
            // Arrange
            var entity = new Member
            {
                MemberID = 101,
                FirstName = "Jane",
                LastName = "Smith",
                BirthDate = new DateOnly(1995, 5, 20),
                Address = "456 Oak St",
                Branch = "Main",
                ContactNo = "555-0199",
                EmailAddress = "jane.smith@email.com",
                IsActive = true,
                DateCreated = DateTime.Now
            };

            // Act
            var dto = entity.ToDto();

            // Assert
            Assert.Equal(entity.MemberID, dto.MemberID);
            Assert.Equal(entity.FirstName, dto.FirstName);
            Assert.Equal(entity.LastName, dto.LastName);
            // Verify DateOnly to DateTime conversion
            Assert.Equal(entity.BirthDate?.Year, dto.BirthDate?.Year);
            Assert.Equal(entity.IsActive, dto.IsActive);
        }

        [Fact]
        public void ToEntity_ShouldMapDtoToEntityCorrectly()
        {
            // Arrange
            var dto = new MemberDTO
            {
                MemberID = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1988, 10, 15),
                Address = "123 Street",
                EmailAddress = "john.doe@email.com"
            };

            // Act
            var entity = dto.ToEntity();

            // Assert
            Assert.Equal(dto.MemberID, entity.MemberID);
            Assert.Equal(dto.FirstName, entity.FirstName);
            // Verify DateTime to DateOnly conversion
            Assert.Equal(DateOnly.FromDateTime(dto.BirthDate.Value), entity.BirthDate);
        }

        [Fact]
        public void ToDto_WithNullBirthDate_ShouldHandleNull()
        {
            // Arrange
            var entity = new Member { BirthDate = null };

            // Act
            var dto = entity.ToDto();

            // Assert
            Assert.Null(dto.BirthDate);
        }

        [Fact]
        public void ToEntity_WithNullBirthDate_ShouldHandleNull()
        {
            // Arrange
            var dto = new MemberDTO { BirthDate = null };

            // Act
            var entity = dto.ToEntity();

            // Assert
            Assert.Null(entity.BirthDate);
        }
    }
}