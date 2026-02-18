namespace MemberManagement.SharedKernel.Constant
{
    public static class OperationMessage
    {
        public static class User
        {
            public const string Created = "Member successfully created.";
            public const string Updated = "Member successfully updated.";
            public const string Deleted = "Member successfully deactivated.";
            public const string Restored = "Member successfully restored.";
        }

        public static class Branch
        {
            public const string Created = "Branch successfully created.";
            public const string Updated = "Branch successfully updated.";
            public const string Deleted = "Branch successfully deactivated.";
        }

        public static class Membership
        {
            public const string Created = "Membership type successfully created.";
            public const string Updated = "Membership type successfully updated.";
            public const string Deleted = "Membership type successfully deleted.";
        }

        public static class Error
        {
            public const string NotFound = "Record not found in database.";
            public const string SaveFailed = "Database save operation failed.";
            public const string RestoreFailed = "Failed to restore member.";
            public const string InvalidInput = "Provided data is invalid.";
            public const string BirthDateRequired = "Birth Date is required.";
            public const string Underage = "Member must be at least 18 years old.";
            public const string ExceedsAgeLimit = "Member exceeds age limit (65y 6m 1d).";
        }
    }
}