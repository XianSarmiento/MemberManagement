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
}