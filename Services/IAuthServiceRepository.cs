using KiwiTaskAPI.Dtos;

namespace KiwiTaskAPI.Services
{
    public interface IAuthServiceRepository
    {
        Task<(bool success, string token, string userName)> IsEmailValidAsync(string email);
        Task<(object result, int statusCode)> RegisterUserAsync(RegisterDto dto);
        Task<(object result, int statusCode)> LoginUserAsync(LoginDto dto);
        Task<bool> IsExistEmail(string email);
        Task<(object result, int statusCode)> RefreshTokenAsync(string refreshToken);
    }
}
