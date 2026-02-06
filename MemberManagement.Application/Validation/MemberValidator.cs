using FluentValidation;
using MemberManagement.Domain.Entities;

// MemberValidator (Domain): Validates the Business Rule. It ensures the actual record is valid before it hits the database.
namespace MemberManagement.Application.Validation
{
    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("First Name is required.");

            RuleFor(m => m.LastName).NotEmpty().WithMessage("Last Name is required.");

            RuleFor(m => m.BirthDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("BirthDate cannot be in the future.")
                .When(m => m.BirthDate.HasValue);

            RuleFor(m => m.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.")
                .When(m => !string.IsNullOrEmpty(m.Address));

            RuleFor(m => m.Branch)
                .MaximumLength(50).WithMessage("Branch cannot exceed 50 characters.")
                .When(m => !string.IsNullOrEmpty(m.Branch));

            RuleFor(m => m.ContactNo)
                .Matches(@"^09\d{9}$").WithMessage("Invalid PH number.")
                .When(m => !string.IsNullOrEmpty(m.ContactNo));

            RuleFor(m => m.EmailAddress)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid email address.")
                .When(m => !string.IsNullOrEmpty(m.EmailAddress));
        }
    }
}

/* HOW IT WORKS:
  This validator acts as a "Gatekeeper" for your Domain Entities. It uses a 
  fluent API to chain rules together in a readable way.

  1. MANDATORY FIELDS: The 'NotEmpty()' rules for FirstName and LastName ensure 
     the database never receives null or whitespace strings for essential identity data.

  2. LOGICAL CONSTRAINTS: The 'BirthDate' rule prevents "impossible" data (like 
     a member born next year). It uses '.When()' to skip this check if the birthdate 
     is null, allowing for optional data without crashing.

  3. DATABASE PROTECTION: By capping 'Address' and 'Branch' lengths (200 and 50 
     characters), you prevent "SQL Truncation Errors" where the data sent is 
     larger than the database column size.

  4. REGULAR EXPRESSIONS (Regex):
     - ContactNo: Specifically checks for Philippines mobile formats (starting 
       with '09' followed by 9 digits).
     - EmailAddress: Uses a pattern to ensure the string follows a standard 
       'user@domain.com' format.

  5. CHAINING & CONDITIONAL LOGIC: Every rule is configured with '.WithMessage()'. 
     This custom text is what gets sent back to the UI when a user fills out 
     a form incorrectly, making it very "user-friendly."
*/