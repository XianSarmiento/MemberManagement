namespace MemberManagement.Application.DTOs
{
    public class MemberDTO
    {
        public int MemberID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Branch { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

/* HOW IT WORKS:
  This class acts as a "Contract" for data exchange. It defines exactly what 
  information is sent over the network when a user requests or saves a member.

  1. DATA ISOLATION: By using a DTO instead of your database "Entity," you 
     protect your internal database structure. If you change a column name 
     in your database, you don't necessarily have to break your API; you 
     just update the Mapper.

  2. NULLABLE TYPES (string? / DateTime?): The use of the '?' marks these 
     fields as optional. This is crucial for "Update" scenarios where a user 
     might leave the address or birthdate blank. It prevents the application 
     from crashing when data is missing.

  3. FLAT STRUCTURE: Unlike complex Domain Entities that might have links to 
     other tables (like 'BranchObject' or 'AuditLogs'), a DTO is "flat." It 
     contains only simple types (strings, ints, bools), which makes it very 
     easy to turn into JSON for web requests.

  4. UI COMPATIBILITY: 
     - BirthDate: Uses 'DateTime' because frontend frameworks (like React 
       or Angular) and JSON serializers handle 'DateTime' better than the 
       C#-specific 'DateOnly' type.
     - DateCreated/IsActive: Includes read-only metadata so the UI can 
       display when the member joined or show a "Disabled" badge.
*/