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
        public string? ContactNo { get; set; }
        public string? EmailAddress { get; set; }

        // System Fields
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        
        public void Initialize()
        {
            this.IsActive = true;
            this.DateCreated = DateTime.UtcNow;
        }


        // Branch Integration
        public int BranchId { get; private set; }
        public void ChangeBranch(int branchId)
        {
            BranchId = branchId;
        }
        public virtual Branch Branch { get; private set; } = null!;


        // Membership Type Integration
        public int MembershipTypeId { get; private set; }
        public virtual MembershipType MembershipType { get; private set; } = null!;


        // Constructor for Branch Feature
        public Member(string firstName, string lastName, DateOnly birthDate, int branchId, int membershipTypeId, string? address = null, string? contactNo = null, string? emailAddress = null)
        {
            ValidateAge(birthDate);

            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            BranchId = branchId;
            MembershipTypeId = membershipTypeId;
            Address = address;
            ContactNo = contactNo;
            EmailAddress = emailAddress;

            IsActive = true;
            DateCreated = DateTime.UtcNow;
        }

        private void ValidateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var latest = today.AddYears(-18);
            var earliest = today.AddYears(-65).AddMonths(-6).AddDays(-1);

            if (birthDate > latest) throw new InvalidOperationException("Member must be at least 18 years old.");
            if (birthDate < earliest) throw new InvalidOperationException("Member exceeds age limit.");
        }
        
        public Member() { }
    }
}
