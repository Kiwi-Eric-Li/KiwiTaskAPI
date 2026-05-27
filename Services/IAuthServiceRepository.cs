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
        Task<Users> GetUserInfo(Guid user_id);
        Task<int> modifyUserInfo(UsersDto dto);
        Task<int> modifyAccountDetail(UsersDto dto);
    }
}
