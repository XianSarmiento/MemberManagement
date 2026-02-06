using System.ComponentModel.DataAnnotations;

namespace MemberManagement.Web.ViewModels
{
    public class MemberVM
    {
        public int MemberID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

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

/* HOW IT WORKS:
  This class represents exactly what the user sees on their screen. It is bound to 
  Create, Edit, and Detail pages.

  1. RAZOR TAG HELPERS: By using this class in a Razor View (e.g., @model MemberVM), 
     ASP.NET can automatically generate HTML input IDs and names that match these 
     properties, making form submission seamless.

  2. UI HINTS ([DataType(DataType.Date)]): This attribute is a "hint" to the 
     browser. When you render an editor for 'BirthDate', the browser will 
     automatically display a native calendar/date-picker instead of a 
     standard text box.

  3. OPTIONALITY: All string fields are nullable ('string?'), which prevents the 
     web server from throwing errors if a user leaves a non-required field blank 
     during a form submission.

  4. READ-ONLY METADATA: 'IsActive' and 'DateCreated' are included here so they 
     can be displayed on a "Details" or "Profile" page, even though the user 
     typically shouldn't be allowed to edit them directly.

  5. CLEAN SEPARATION: By keeping this in the Web project, you can add 
     web-specific attributes (like display names or custom formatting) 
     without cluttering your core business logic.
*/