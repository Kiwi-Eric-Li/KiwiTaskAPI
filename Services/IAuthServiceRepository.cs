using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Services
{
    public interface IAuthServiceRepository
    {
        Task<bool> IsUserExist(Guid user_id);
        Task<(bool success, string token, string userName)> IsEmailValidAsync(string email);
        Task<(object result, int statusCode)> RegisterUserAsync(RegisterDto dto);
        Task<(object result, int statusCode)> LoginUserAsync(LoginDto dto);
        Task<bool> IsExistEmail(string email);
        Task<(object result, int statusCode)> RefreshTokenAsync(string refreshToken);
        Task<Users> GetUserInfoAsync(Guid user_id);
        Task<int> modifyUserInfoAsync(UsersDto dto);
        Task<int> modifyAccountDetailAsync(UsersDto dto);
        Task<int> modifyPasswordAsync(ModifyPasswordDto dto);

        Task<List<PreferredCategories>> GetUserPreferredCategories(Guid user_id);
        Task<int> ModifyUserPreferredCategories(List<PreferredCategories> preferredCategoriesList);
        Task<int> ModifyNotificationSettings(Guid userId, Dictionary<string, int> data);

        Task<NotificationSettings> GetNotificationSettings(Guid user_id);
    }
}
