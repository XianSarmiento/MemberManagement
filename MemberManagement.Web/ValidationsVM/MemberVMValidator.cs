using FluentValidation;
using MemberManagement.Domain.Entities;
using MemberManagement.Web.ViewModels;

// MemberVMValidator (Web): Validates the User Input. It ensures the form is filled out correctly.
namespace MemberManagement.Web.ValidationsVM
{
    public class MemberVMValidator : AbstractValidator<MemberVM>
    {
        public MemberVMValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");

            RuleFor(x => x.BirthDate)
                .Must(date => !date.HasValue || date.Value.Date <= DateTime.Today)
                .WithMessage("Birth date cannot be in the future");

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
  This validator focuses on the 'MemberVM' (View Model). Its job is to provide 
  immediate feedback to the user after they click "Submit" on a web form.

  1. WEB-SPECIFIC VALIDATION: It targets 'MemberVM', which is the class bound 
     to your HTML forms. This allows the ASP.NET Controller to catch errors 
     early and send them back to the View to be displayed in red text.

  2. USER EXPERIENCE (UX): By defining messages like "First Name is required," 
     you are translating raw data requirements into human-readable instructions. 
     This is the first line of defense in the user interface.

  3. BIRTH DATE LOGIC: It uses the '.Must()' method for a custom check. 
     It verifies that if a date is entered, it isn't in the future. 
     This is critical for data integrity before the data is ever 
     converted to a DTO or Entity.

  4. STRING CONSTRAINTS:
     - Address/Branch: Prevents users from entering too much text, 
       which might break the UI layout or exceed database limits.
     - ContactNo: Uses a specific Regular Expression (Regex) to ensure 
       the phone number follows the Philippines mobile format (09XXXXXXXXX).

  5. CONDITIONAL CHECKS (.When): It only runs length and format checks if 
      the field isn't empty. This prevents "double errors" where a user 
     sees both "Field is required" and "Format is invalid" at the same time.
*/