using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Web.ViewModels
{
    public class MemberVM
    {
        public int MemberID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Please select a branch")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public string? Branch { get; set; }

        [Required(ErrorMessage = "Please select a membership type")]
        [Display(Name = "Membership Type")]
        public int MembershipTypeId { get; set; }
        public string? MembershipType { get; set; }

        [Display(Name = "Contact Number")]
        public string? ContactNo { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        // Include these only if displaying them
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}