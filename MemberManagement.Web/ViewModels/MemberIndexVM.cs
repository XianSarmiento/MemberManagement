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
    {
        if (SortColumn == column)
        {
            return SortOrder == "asc" ? "desc" : "asc";
        }
        return "asc";
    }

}
