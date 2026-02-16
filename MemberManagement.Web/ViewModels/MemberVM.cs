using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Web.ViewModels
{
    public class MemberVM
    {
        public int MemberID { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        public string? Address { get; set; }

        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        public string? Branch { get; set; }

        [Display(Name = "Membership Type")]
        public int MembershipTypeId { get; set; }

        public string? MembershipType { get; set; }

        [Display(Name = "Contact Number")]
        public string? ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}