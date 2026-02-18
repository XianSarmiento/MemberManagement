namespace MemberManagement.Domain.Entities
{
    public class MembershipType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string MembershipCode { get; private set; }
        public string Description { get; private set; }
        public decimal MembershipFee { get; private set; }
        public bool IsActive { get; private set; }

        public MembershipType(string name, string membershipCode, decimal fee, string description = "")
        {
            Update(name, membershipCode, fee, description);
            IsActive = true;
        }

        public void Update(string name, string membershipCode, decimal fee, string description)
        {
            Name = name;
            MembershipCode = membershipCode;
            MembershipFee = fee;
            Description = description;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        protected MembershipType() { }
    }
}