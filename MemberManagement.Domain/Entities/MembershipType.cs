namespace MemberManagement.Domain.Entities
{
    public class MembershipType
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string MembershipCode { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public decimal MembershipFee { get; private set; }
        public bool IsActive { get; private set; }


        // MAIN CONSTRUCTOR: This is your primary "Gatekeeper"
        public MembershipType(string name, string membershipCode, decimal fee, string description = "")
        {
            Update(name, membershipCode, fee, description);
            IsActive = true;
        }

        // ENCAPSULATION: Logic for modifying existing objects
        public void Update(string name, string membershipCode, decimal fee, string description)
        {
            // Basic validation to prevent nulls during runtime
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MembershipCode = membershipCode ?? throw new ArgumentNullException(nameof(membershipCode));
            MembershipFee = fee;
            Description = description ?? string.Empty;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        // PROTECTED CONSTRUCTOR: This satisfies the 'Non-nullable' error
        protected MembershipType() { }
    }
}