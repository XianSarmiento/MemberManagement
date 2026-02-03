using MemberManagement.Domain.Entities;
using Xunit;
using MemberManagement.Tests.Extensions;


namespace MemberManagement.Tests.Domain
{
    public class MemberTests
    {
        [Fact]
        public void FullName_Should_Return_FirstName_And_LastName()
        {
            var member = new Member
            {
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = new DateOnly(2000, 1, 1),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "john@example.com"
            };

            var fullName = member.FullName();

            Xunit.Assert.Equal("John Sarmiento", fullName);
        }

        [Fact]
        public void IsAdult_Should_Return_True_If_18_Or_Older()
        {
            var member = new Member
            {
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = new DateOnly(2000, 1, 1),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "john@example.com"
            };

            Xunit.Assert.True(member.IsAdult());
        }
    }
}
