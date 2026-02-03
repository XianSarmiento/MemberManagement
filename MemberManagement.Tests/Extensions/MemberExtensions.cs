using MemberManagement.Domain.Entities;

namespace MemberManagement.Tests.Extensions
{
    public static class MemberExtensions
    {
        public static string FullName(this Member m) => $"{m.FirstName} {m.LastName}";

        public static bool IsAdult(this Member m)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - m.BirthDate.Year;
            if (m.BirthDate > today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}
