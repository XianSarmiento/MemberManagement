using MemberManagement.SharedKernel.Constant;
using Xunit;
using Assert = Xunit.Assert;



namespace MemberManagement.UnitTests.Application.Core
{
    public class OperationMessageTests
    {
        [Fact]
        public void UserMessages_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal("Member successfully created.", OperationMessage.User.Created);
            Assert.Equal("Member successfully updated.", OperationMessage.User.Updated);
            Assert.Equal("Member successfully deactivated.", OperationMessage.User.Deleted);
        }

        [Fact]
        public void ErrorMessages_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal("Member record not found in database.", OperationMessage.Error.NotFound);
            Assert.Equal("Database save operation failed for Member entity.", OperationMessage.Error.SaveFailed);
            Assert.Equal("Provided member data is invalid.", OperationMessage.Error.InvalidInput);
        }
    }
}