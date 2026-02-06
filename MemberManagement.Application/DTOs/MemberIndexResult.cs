namespace MemberManagement.Application.DTOs
{
    public class MemberIndexResult
    {
        public List<MemberDTO> Members { get; set; } = new();
        public List<string> Branches { get; set; } = new();
        public int TotalItems { get; set; }
    }
}

/* HOW IT WORKS:
  This class serves as a "wrapper" or "container" for the data returned to a 
  list view (Index page). It solves the problem of needing to pass more than 
  just a simple list of names to the user interface.

  1. DATA BUNDLING: Instead of making three separate calls to the server—one 
     for the members, one for the total count, and one for the filter 
     options—this object packages them together. This reduces network 
     traffic and improves performance.

  2. UI SUPPORT (Branches): The 'Branches' list is specifically included to 
     populate "Filter by Branch" dropdown menus. It allows the UI to show 
     available options dynamically based on the data currently in the system.

  3. PAGINATION READY (TotalItems): The 'TotalItems' property is essential 
     for UI components like paginators (e.g., "Showing 1-10 of 500"). Even 
     if the 'Members' list only contains 10 items for the current page, 
     this property tells the UI how many pages exist in total.

  4. NULL SAFETY: By initializing the lists with '= new();', the code 
     prevents "Null Reference Exceptions." If no members are found, the UI 
     will receive an empty list '[]' instead of a null value, which is 
     much safer for loops and frontend rendering.
*/