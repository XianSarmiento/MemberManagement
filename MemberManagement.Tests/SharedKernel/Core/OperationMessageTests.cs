using FluentAssertions;
using MemberManagement.SharedKernel.Constant;
using Xunit;

namespace MemberManagement.Test.SharedKernel
{
    public class OperationMessageTests
    {
        [Fact]
        public void UserMessages_ShouldHaveCorrectStrings()
        {
            // Assert
            // This ensures that if someone accidentally changes "Member" to "User", the test fails.
            OperationMessage.User.Created.Should().Be("Member successfully created.");
            OperationMessage.User.Deleted.Should().Be("Member successfully deactivated.");
        }

        [Fact]
        public void ErrorMessages_ShouldHaveCorrectStrings()
        {
            // Assert
            OperationMessage.Error.NotFound.Should().Be("Record not found in database.");
            OperationMessage.Error.Underage.Should().Be("Member must be at least 18 years old.");
        }

        [Theory]
        [InlineData(OperationMessage.User.Created)]
        [InlineData(OperationMessage.Branch.Created)]
        [InlineData(OperationMessage.Membership.Created)]
        public void AllCreatedMessages_ShouldNotBeNullOrEmpty(string message)
        {
            // This is a "Theory" test to check multiple constants at once
            message.Should().NotBeNullOrEmpty();
        }
    }
}