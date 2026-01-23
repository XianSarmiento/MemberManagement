using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Domain.Entities
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        // Core Member Data
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public required string Address { get; set; }
        public required string Branch { get; set; }

        // Additonal Member Fields
        public required string ContactNo { get; set; }
        public required string EmailAddress { get; set; }
        public bool IsActive { get; set; } 
        public DateTime DateCreated { get; set; }

    }
}
