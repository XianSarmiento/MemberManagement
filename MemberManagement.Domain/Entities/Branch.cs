namespace MemberManagement.Domain.Entities
{
    public class Branch
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Address { get; private set; } = null!;
        public string BranchCode { get; private set; } = null!;
        public bool IsActive { get; private set; }


        // CONSTRUCTOR: Enforces that a Branch must have a Name and Code to exist
        public Branch(string name, string address, string branchCode)
        {
            // GUARD CLAUSES: Validation within constructor
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Branch name is required.");

            if (string.IsNullOrWhiteSpace(branchCode))
                throw new ArgumentException("Branch code is required.");

            Name = name;
            Address = address ?? string.Empty;
            BranchCode = branchCode;
            IsActive = true; // DEFAULT VALUE: New branches are active by default
        }

        // PROTECTED CONSTRUCTOR: Used by ORM for proxying or hydration
        protected Branch() { }

        public void Update(string name, string address, string branchCode)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(branchCode))
                throw new ArgumentException("Branch code cannot be empty.");

            Name = name;
            Address = address ?? string.Empty;
            BranchCode = branchCode;
        }

        public void Activate() => IsActive = true;

        public void Deactivate() => IsActive = false;
    }
}