namespace MemberManagement.Application.DTOs
{
    public class MembershipTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MembershipCode { get; set; }
        public string Description { get; set; }
        public decimal MembershipFee { get; set; }
        public bool IsActive { get; set; }
    }
}