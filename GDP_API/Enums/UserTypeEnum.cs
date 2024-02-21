namespace GDP_API
{
    /// <summary>
    /// Represents the type of a user. Normal, expert or admin.
    /// </summary>
    public enum UserType
    {
        //All should never be assigned to a user,
        // it is used to get all users in a project and similar queries
        All = 0,
        Normal = 1,
        Expert = 2,
        Admin = 3,
    }
}