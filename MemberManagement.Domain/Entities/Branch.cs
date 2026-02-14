namespace MemberManagement.Domain.Entities
{
    public class Branch
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }

        // Constructor for enforcing valid object creation
        public Branch(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Branch name is required.");

            Name = name;
            IsActive = true;
        }

        // Required for EF Core
        protected Branch() { }

        public void UpdateName(string newName) => Name = newName;
        public void Deactivate() => IsActive = false;
    }
}