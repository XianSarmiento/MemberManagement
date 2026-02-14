namespace MemberManagement.Domain.Entities
{
    public class MembershipType
    {
        public int Id { get; private set; }
        public string Name { get; private set; } // e.g., "Regular", "Associate"
        public decimal MembershipFee { get; private set; }

        public MembershipType(string name, decimal fee)
        {
            Name = name;
            MembershipFee = fee;
        }

        protected MembershipType() { }
    }
}