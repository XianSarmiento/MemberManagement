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

    // NEW: The View just calls this property instead of running IF statements
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

    // NEW: Helper for the View to determine the next sort order
    public string GetToggleOrder(string column)
        => (SortColumn == column && SortOrder == "asc") ? "desc" : "asc";
}