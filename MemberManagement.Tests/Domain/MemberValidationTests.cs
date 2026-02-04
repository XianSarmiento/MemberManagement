using MemberManagement.Domain.Entities;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.Tests.Domain
{
    public class MemberValidationTests
    {
        [Fact]
        public void Email_Should_Be_Valid()
        {
            var member = new Member
            {
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = new DateOnly(2000, 1, 1),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "john@example.com" // valid email
            };

            bool isValid = member.IsValidEmail();

            Assert.True(isValid);
        }

        [Fact]
        public void Email_Should_Be_Invalid()
        {
            var member = new Member
            {
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = new DateOnly(2000, 1, 1),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "invalid-email" // invalid email
            };

            bool isValid = member.IsValidEmail();

            Assert.False(isValid); // now the test will pass
        }
    }

    // Extension for testing
    public static class MemberValidationExtensions
    {
        public static bool IsValidEmail(this Member m)
        {
            if (string.IsNullOrWhiteSpace(m.EmailAddress))
                return false; // null or empty is invalid

            try
            {
                var addr = new System.Net.Mail.MailAddress(m.EmailAddress);
                return addr.Address == m.EmailAddress;
            }
            catch
            {
                return false;
            }
        }
    }
}
