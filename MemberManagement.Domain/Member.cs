using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Domain
{
    public class Member
    {
        // Primary Key
        [Key]
        public int MemberID { get; set; }

        // Core Member Data
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // public DateOnly BirthDate { get; set; }
        public DateTime BirthDate { get; set; }

        public string Address { get; set; }
        public string Branch { get; set; }

        // Additonal Member Fields
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; } 
        public DateTime DateCreated { get; set; }
    }
}
