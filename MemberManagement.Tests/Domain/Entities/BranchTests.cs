using MemberManagement.Domain.Entities; // FIX 1: Add this
using FluentAssertions;
using Xunit;
using System; // Required for Action

namespace MemberManagement.UnitTests.Domain.Entities
{
    public class BranchTests
    {
        [Fact]
        public void Constructor_ValidData_ShouldSetIsActiveTrue()
        {
            var branch = new Branch("Main Branch", "123 St", "MB001");

            branch.IsActive.Should().BeTrue();
            branch.BranchCode.Should().Be("MB001");
        }

        [Theory]
        [InlineData("", "CODE")]
        [InlineData("Name", "")]
        [InlineData(null, "CODE")] // This triggers the warning
        // FIX 2: Change 'string' to 'string?' to allow null inputs in the test
        public void Constructor_InvalidData_ShouldThrowArgumentException(string? name, string? code)
        {
            // Use '!' to tell the compiler "I know I'm passing null/empty, that's the point"
            Action act = () => new Branch(name!, "Address", code!);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Deactivate_ShouldChangeStatus()
        {
            var branch = new Branch("Main", "Address", "M01");

            branch.Deactivate();
            branch.IsActive.Should().BeFalse();

            branch.Activate();
            branch.IsActive.Should().BeTrue();
        }
    }
}