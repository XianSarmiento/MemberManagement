using MemberManagement.Application.Business;
using MemberManagement.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.UnitTests.Application
{
    public class MemberExportServiceTests
    {
        private readonly MemberExportService _exportService;

        public MemberExportServiceTests()
        {
            _exportService = new MemberExportService();
        }

        private List<MemberDTO> GetTestMembers()
        {
            return new List<MemberDTO>
            {
                new MemberDTO
                {
                    MemberID = 1,
                    FirstName = "John Christian",
                    LastName = "Sarmiento",
                    Branch = "Head Office",
                    BirthDate = new DateTime(1990, 1, 1),
                    Address = "Pantino Street",
                    ContactNo = "09876620232",
                    EmailAddress = "jcmsarmiento@gmail.com"
                },
                new MemberDTO
                {
                    MemberID = 2,
                    FirstName = "Xian",
                    LastName = "Manoguid",
                    Branch = "Branch 1",
                    BirthDate = new DateTime(1985, 5, 10),
                    Address = "456 Avenue",
                    ContactNo = "09123456789",
                    EmailAddress = "xian@gmail.com"
                }
            };
        }

        [Fact]
        public void GenerateExcel_ShouldReturnNonEmptyByteArray()
        {
            // Arrange
            var members = GetTestMembers();

            // Act
            var result = _exportService.GenerateExcel(members);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GeneratePdf_ShouldReturnNonEmptyByteArray()
        {
            // Arrange
            var members = GetTestMembers();

            // Act
            var result = _exportService.GeneratePdf(members);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);

            // Optional: Check PDF header
            var pdfHeader = Encoding.ASCII.GetString(result, 0, 4);
            Assert.Equal("%PDF", pdfHeader);
        }
    }
}
