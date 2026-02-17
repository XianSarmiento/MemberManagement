namespace MemberManagement.Application.DTOs
{
    public class BranchDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!; 
        public string BranchCode { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}