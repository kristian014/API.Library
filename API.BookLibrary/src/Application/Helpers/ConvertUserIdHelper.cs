using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.User;

namespace Application.Helpers
{
    public static class ConvertUserIdHelper
    {
        public static Guid GetConvertedUserId(IUserContext? userContext)
        {
            CurrentUser? user = userContext?.GetCurrentUser();
            _ = user ?? throw new NotFoundException("Current User was not found");
            if (Guid.TryParse(user.id, out Guid convertedUserId))
            {
                return convertedUserId;
            }
            else
            {
                throw new Exception($"Unable to convert userId to string {user.id}");
            }
        }
    }
}
