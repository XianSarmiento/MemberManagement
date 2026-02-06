namespace MemberManagement.Application.Core
{
    public static class OperationMessage
    {
        public static class User
        {
            public const string Created = "Member successfully created.";
            public const string Updated = "Member successfully updated.";
            public const string Deleted = "Member successfully deactivated.";
        }

        public static class Error
        {
            public const string NotFound = "Member record not found in database.";
            public const string SaveFailed = "Database save operation failed for Member entity.";
            public const string InvalidInput = "Provided member data is invalid.";
        }
    }
}

/* HOW IT WORKS:
  This static class acts as a single source of truth for all status and error 
  messages in the application.

  1. ELIMINATING "MAGIC STRINGS": Instead of typing "Member not found" manually in 
     five different handlers, you use 'OperationMessage.Error.NotFound'. This 
     prevents typos and ensures the user sees the exact same message every time.

  2. EASY MAINTENANCE: If your boss decides the message should say "User deactivated" 
     instead of "Member deactivated," you only have to change it in this one file 
     rather than searching through your entire codebase.

  3. ORGANIZED HIERARCHY: By using nested static classes ('User' and 'Error'), 
     the code becomes self-documenting. When you type 'OperationMessage.', 
     IntelliSense (auto-complete) helps you find exactly what you need quickly.

  4. CONST VS READONLY: These are defined as 'const', meaning they are set at 
     compile-time and cannot be changed during the program's execution. This 
     is memory-efficient and ideal for static status messages.

  5. CLEANER TESTING: When writing unit tests, you can compare the output of 
     your code against these constants to ensure the correct error path 
     was triggered.
*/