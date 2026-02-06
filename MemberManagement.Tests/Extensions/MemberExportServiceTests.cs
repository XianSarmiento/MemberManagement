using MemberManagement.Application.DTOs;
using MemberManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace MemberManagement.UnitTests.Application.Extensions
{
    public class MemberExportServiceTests
    {

        [Fact]
        public void GenerateExcel_ShouldReturnNonEmptyByteArray_WhenMembersExist()
        {
            // Arrange
            var members = new List<MemberDTO>
            {
                new MemberDTO { MemberID = 1, FirstName = "John", LastName = "Doe", EmailAddress = "john@example.com" }
            };
        }

        [Fact]
        public void GeneratePdf_ShouldReturnNonEmptyByteArray_WhenMembersExist()
        {
            // Arrange
            var members = new List<MemberDTO>
            {
                new MemberDTO { MemberID = 1, FirstName = "Jane", LastName = "Smith" }
            };
        }

        [Fact]
        public void GenerateExcel_ShouldHandleEmptyList()
        {
            // Arrange
            var members = new List<MemberDTO>();
        }
    }
}