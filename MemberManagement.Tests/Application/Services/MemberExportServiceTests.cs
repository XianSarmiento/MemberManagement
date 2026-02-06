using MemberManagement.Application.DTOs;
using MemberManagement.Application.Services;
using System;
using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Application.Services
{
    public class MemberExportServiceTests
    {
        private readonly MemberExportService _service;

        public MemberExportServiceTests()
        {
            _service = new MemberExportService();
        }

        private List<MemberDTO> GetSampleMembers()
        {
            return new List<MemberDTO>
            {
                new MemberDTO
                {
                    MemberID = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Branch = "Main",
                    BirthDate = new DateTime(1990, 1, 1),
                    EmailAddress = "john@example.com"
                }
            };
        }

        [Fact]
        public void GenerateExcel_ShouldReturnValidByteArray()
        {
            // Arrange
            var members = GetSampleMembers();

            // Act
            var result = _service.GenerateExcel(members);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0, "Excel file should not be empty.");

            // Optional: Check for Excel magic number (PK..) 
            // Most modern Excel files (xlsx) start with 'P' and 'K' (ZIP format)
            Assert.Equal('P', (char)result[0]);
            Assert.Equal('K', (char)result[1]);
        }

        [Fact]
        public void GeneratePdf_ShouldReturnValidByteArray()
        {
            // Arrange
            var members = GetSampleMembers();

            // Act
            var result = _service.GeneratePdf(members);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0, "PDF file should not be empty.");

            // Check for PDF magic number (%PDF-)
            Assert.Equal('%', (char)result[0]);
            Assert.Equal('P', (char)result[1]);
            Assert.Equal('D', (char)result[2]);
            Assert.Equal('F', (char)result[3]);
        }

        [Fact]
        public void GenerateExcel_WithEmptyList_ShouldNotThrow()
        {
            // Arrange
            var members = new List<MemberDTO>();

            // Act & Assert
            var result = Record.Exception(() => _service.GenerateExcel(members));
            Assert.Null(result);
        }
    }
}