using MemberManagement.Web.ViewModels;
using X.PagedList;

public class MemberIndexVM
{
    public IPagedList<MemberVM> Members { get; set; } = null!;
    public string SearchLastName { get; set; } = "";
    public string SelectedBranch { get; set; } = "";
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "MemberID";
    public string SortOrder { get; set; } = "asc";
    public List<string> Branches { get; set; } = new();

    public string NoResultsMessage
    {
        get
        {
            if (!Members.Any() && string.IsNullOrWhiteSpace(SearchLastName) && string.IsNullOrWhiteSpace(SelectedBranch))
                return "No records available at the moment.";

            if (!string.IsNullOrWhiteSpace(SearchLastName) && string.IsNullOrWhiteSpace(SelectedBranch))
                return $"No members found matching \"{SearchLastName}\".";

            if (string.IsNullOrWhiteSpace(SearchLastName) && !string.IsNullOrWhiteSpace(SelectedBranch))
                return $"No branch filtered named \"{SelectedBranch}\".";

            if (!string.IsNullOrWhiteSpace(SearchLastName) && !string.IsNullOrWhiteSpace(SelectedBranch))
                return $"No members found matching \"{SearchLastName}\" in branch \"{SelectedBranch}\".";

            return "No results for your current filters.";
        }
    }

    public string GetToggleOrder(string column)
        => (SortColumn == column && SortOrder == "asc") ? "desc" : "asc";
}

/* HOW IT WORKS:
  This ViewModel is the "State" of your Index page. It holds the data and the 
  logic used to control how the table and filters behave.

  1. PAGINATION (IPagedList): Instead of a simple List, it uses 'IPagedList'. 
     This interface contains metadata like 'PageCount', 'HasNextPage', and 
     'PageNumber', which allows you to render pagination controls (1, 2, 3...) 
     automatically in the view.

  2. DYNAMIC MESSAGING (NoResultsMessage): This is a "calculated property." 
     Rather than putting complex 'if' statements in your HTML, this property 
     looks at the search filters and the member count to generate a 
     human-friendly message. This keeps your HTML files much cleaner.

  3. SORTING LOGIC (GetToggleOrder): This helper method simplifies the header 
     links in your table. When a user clicks "Last Name," the view calls this 
     method to decide if the next click should sort ascending or descending.

  4. FILTER PERSISTENCE: By storing 'SearchLastName' and 'SelectedBranch', 
     the search bar and dropdown menu will "remember" what the user typed 
     after the page reloads.

  5. UI DATA (Branches): This list is used to populate the filter dropdown, 
     ensuring that only existing branches appear as options for the user.
*/