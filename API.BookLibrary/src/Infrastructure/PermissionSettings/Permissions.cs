using System.Collections.ObjectModel;

namespace Shared.PermissionSettings
{
    public static class APIAction
    {
        public const string View = nameof(View);
        public const string Search = nameof(Search);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
    }

    public static class Resource
    {
        public const string Books = nameof(Books);
        public const string CoverType = nameof(CoverType);
        public const string Category = nameof(Category);
        public const string Author = nameof(Author);
    }

    public static class Permissions
    {
        private static readonly Permission[] _all = new Permission[]
        {
            new("View Books", APIAction.View, Resource.Books, IsBasic: true),
            new("Search Books", APIAction.Search, Resource.Books, IsBasic: true),
            new("Create Books", APIAction.Create, Resource.Books, IsAdmin: true),
            new("Update Books", APIAction.Update, Resource.Books, IsAdmin: true),
            new("Delete Books", APIAction.Delete, Resource.Books),

            new("View Authors", APIAction.View, Resource.Author, IsAdmin: true),
            new("Search Authors", APIAction.Search, Resource.Author, IsAdmin: true),
            new("Create Authors", APIAction.Create, Resource.Author, IsAdmin: true),
            new("Update Authors", APIAction.Update, Resource.Author, IsAdmin: true),
            new("Delete Authors", APIAction.Delete, Resource.Author),
        };

        public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);
        public static IReadOnlyList<Permission> Root { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => !p.IsRoot).ToArray());
        public static IReadOnlyList<Permission> Admin { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => !p.IsRoot).ToArray());
        public static IReadOnlyList<Permission> Basic { get; } = new ReadOnlyCollection<Permission>(_all.Where(p => p.IsBasic).ToArray());
    }

    public record Permission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false, bool IsAdmin = false)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }
}
