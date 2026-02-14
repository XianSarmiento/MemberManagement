namespace MemberManagement.Application.DTOs
{
    public class MembershipTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal MembershipFee { get; set; }
    }
}