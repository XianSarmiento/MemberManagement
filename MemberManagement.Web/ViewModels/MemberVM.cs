using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Web.ViewModels
{
    public class MemberVM
    {
        public int MemberID { get; set; }
        public string? FirstName { get; set; }
        public string?  LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Branch { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailAddress { get; set; }


        // Include these only if displaying them
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
