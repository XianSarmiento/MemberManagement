namespace MemberManagement.Application.DTOs
{
    public class MemberIndexResult
    {
        public List<MemberDTO> Members { get; set; } = new();
        public List<string> Branches { get; set; } = new();
        public int TotalItems { get; set; }
    }
}