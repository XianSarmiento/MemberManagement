using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Domain.Entities
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }


        // Core Member Data
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }


        // Optional Member Fields → allow NULL
        public string? Address { get; set; }
        public string? Branch { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailAddress { get; set; }


        // System Fields
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
