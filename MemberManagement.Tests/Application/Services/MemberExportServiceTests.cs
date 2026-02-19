using FluentAssertions;
using MemberManagement.Application.DTOs;
using MemberManagement.Application.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace MemberManagement.Test.Services
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
                    EmailAddress = "john@example.com",
                    MembershipType = "Regular"
                },
                new MemberDTO
                {
                    MemberID = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Branch = "West",
                    BirthDate = new DateTime(1985, 5, 20),
                    EmailAddress = "jane@example.com",
                    MembershipType = "Premium"
                }
            };
        }

        [Fact]
        public void GenerateExcel_ShouldReturnByteArray()
        {
            // Arrange
            var members = GetSampleMembers();

            // Act
            var result = _service.GenerateExcel(members);

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);
            // Optional: Check for Excel magic number (first few bytes)
            result[0].Should().Be(0x50); // 'P' (from PK zip header used by .xlsx)
            result[1].Should().Be(0x4B); // 'K'
        }

        [Fact]
        public void GeneratePdf_ShouldReturnByteArray()
        {
            // Arrange
            var members = GetSampleMembers();

            // Act
            var result = _service.GeneratePdf(members);

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);
            // Check for PDF magic number (%PDF-)
            System.Text.Encoding.ASCII.GetString(result, 0, 4).Should().Be("%PDF");
        }

        [Fact]
        public void GenerateExcel_WithEmptyList_ShouldStillGenerateFile()
        {
            // Act
            var result = _service.GenerateExcel(new List<MemberDTO>());

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);
        }
    }
}