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

        public static class System
        {
            public const string Created = "CREATE operation executed on Member entity.";
            public const string Updated = "UPDATE operation executed on Member entity.";
            public const string Deleted = "DELETE/DEACTIVATE operation executed on Member entity.";
        }

        public static class Error
        {
            public const string NotFound = "Member record not found in database.";
            public const string SaveFailed = "Database save operation failed for Member entity.";
            public const string InvalidInput = "Provided member data is invalid.";
        }
    }
}
