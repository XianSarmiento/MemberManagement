namespace MemberManagement.Application.Business
{
    public class MemberIndexResult
    {
        public List<MemberDTO> Members { get; set; } = new();
        public List<string> Branches { get; set; } = new();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
