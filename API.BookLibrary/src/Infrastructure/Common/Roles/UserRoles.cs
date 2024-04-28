using System.Collections.ObjectModel;

namespace Infrastructure.Common.Roles
{
    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string Basic = nameof(Basic);

        public static IReadOnlyList<string> DefaultUserRoles { get; } = new ReadOnlyCollection<string>(new[]
        {
            Admin,
            Basic
        });

        public static bool IsDefault(string roleName) => DefaultUserRoles.Any(r => r == roleName);
    }
}
