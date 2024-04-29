namespace Application.User
{
    public record CurrentUser(string id, string Email, IEnumerable<string> Roles)
    {
        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
