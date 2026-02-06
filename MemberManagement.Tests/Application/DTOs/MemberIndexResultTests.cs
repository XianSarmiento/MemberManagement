using MemberManagement.Application.DTOs;
using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;



namespace MemberManagement.UnitTests.Application.DTOs
{
    public class MemberIndexResultTests
    {
        [Fact]
        public void MemberIndexResult_Initialization_ShouldNotBeNull()
        {
            // Act
            var result = new MemberIndexResult();

            // Assert - Verify that the lists are instantiated by default
            Assert.NotNull(result.Members);
            Assert.NotNull(result.Branches);
            Assert.Empty(result.Members);
            Assert.Empty(result.Branches);
        }

        [Fact]
        public void MemberIndexResult_ShouldSetAndGetPropertiesCorrectly()
        {
            // Arrange
            var result = new MemberIndexResult();
            var memberList = new List<MemberDTO>
            {
                new MemberDTO { MemberID = 1, FirstName = "Alice" },
                new MemberDTO { MemberID = 2, FirstName = "Bob" }
            };
            var branchList = new List<string> { "Main", "North", "South" };
            int total = 25;

            // Act
            result.Members = memberList;
            result.Branches = branchList;
            result.TotalItems = total;

            // Assert
            Assert.Equal(2, result.Members.Count);
            Assert.Equal("Alice", result.Members[0].FirstName);
            Assert.Equal(3, result.Branches.Count);
            Assert.Contains("North", result.Branches);
            Assert.Equal(25, result.TotalItems);
        }
    }
}