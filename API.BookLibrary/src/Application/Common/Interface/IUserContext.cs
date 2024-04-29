using Application.User;

namespace Application.Common.Interface
{
    public interface IUserContext : IScopedService
    {
        CurrentUser? GetCurrentUser();
    }
}
