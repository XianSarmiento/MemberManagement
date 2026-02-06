using MemberManagement.Domain.Entities;

namespace MemberManagement.Application.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetActiveMembersAsync();
        Task<Member?> GetByIdAsync(int id);
        Task CreateAsync(Member member);
        Task UpdateAsync(Member member);
        Task DeleteAsync(int id);
    }
}

/* HOW IT WORKS:
  This interface defines 'what' the system can do with members without 
  dictating 'how' it does it. This is a core principle of Abstraction.

  1. THE CONTRACT: Any class that implements 'IMemberService' (like a 
     'MemberService' class in your Infrastructure layer) is legally 
     obligated by the compiler to provide code for these five methods.

  2. ASYNCHRONOUS DESIGN: Every method returns a 'Task'. This ensures 
     the application remains responsive by not blocking threads while 
     waiting for database or network operations to complete.

  3. METHOD ROLES:
     - GetActiveMembersAsync: Fetches only "Active" records, likely 
       filtering out soft-deleted or deactivated members.
     - GetByIdAsync: Returns a 'Member?' (nullable), acknowledging 
       that a member might not exist for a given ID.
     - Create/Update/Delete: Standard persistence operations.

  4. DECOUPLING: Because your Handlers (CreateMemberHandler, etc.) 
     depend on this interface instead of a specific database class, 
     you can easily swap the database. For example, you could switch 
     from SQL Server to MongoDB or a Mock testing service without 
     changing a single line of code in your Handlers.
*/