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
            var entity = new Member(
                firstName: "Jane",
                lastName: "Smith",
                birthDate: new DateOnly(1995, 5, 20),
                branchId: 1,        
                membershipTypeId: 1,
                address: "456 Oak St",
                contactNo: "555-0199",
                emailAddress: "jane.smith@email.com"
            )
            {
                MemberID = 101
            };

            // Act
            var dto = entity.ToDto();

            // Assert
            Assert.Equal(entity.MemberID, dto.MemberID);
            Assert.Equal(entity.FirstName, dto.FirstName);
            Assert.Equal(entity.LastName, dto.LastName);
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