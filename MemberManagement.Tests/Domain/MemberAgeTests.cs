using MemberManagement.Domain.Entities;
using MemberManagement.Tests.Extensions;
using System;
using Xunit;
using Assert = Xunit.Assert;

namespace MemberManagement.Tests.Domain
{
    public class MemberAgeTests
    {
        [Fact]
        public void IsAdult_Should_Return_False_If_Under_18()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var member = new Member
            {
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = today.AddYears(-17),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "john@example.com"
            };

            Assert.False(member.IsAdult());
        }

        [Fact]
        public void IsAdult_Should_Return_True_If_Exactly_18()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var member = new Member
            {
                FirstName = "John",
                LastName = "Sarmiento",
                BirthDate = today.AddYears(-18),
                Address = "123 Street",
                Branch = "Main",
                ContactNo = "09171234567",
                EmailAddress = "john@example.com"
            };

            Assert.True(member.IsAdult());
        }
    }
}
