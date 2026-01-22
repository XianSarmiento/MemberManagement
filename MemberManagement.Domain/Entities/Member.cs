using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Domain.Entities
{
    public class Member
    {
        // Primary Key
        [Key]
        public int MemberID { get; set; }

        // Core Member Data
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [BirthDateValidation(ErrorMessage = "BirthDate cannot be in the future.")]
        public DateOnly BirthDate { get; set; }

        public string Address { get; set; }
        public string Branch { get; set; }

        // Additonal Member Fields
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; } 
        public DateTime DateCreated { get; set; }


        // Custom Validation Attribute for BirthDate
        private class BirthDateValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is DateOnly birthDate)
                {
                    if (birthDate > DateOnly.FromDateTime(DateTime.Today))
                    {
                        return new ValidationResult(ErrorMessage ?? "Birthdate cannot be in the future.");
                    }
                }
                return ValidationResult.Success;
            }
        }
    }
}
