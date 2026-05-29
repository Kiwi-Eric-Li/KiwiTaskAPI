
using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;

namespace KiwiTaskAPI.Services
{
    public class AuthServiceRepository : IAuthServiceRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthServiceRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        private (string, DateTime expiry) GenerateTokenByEmail(string email)
        {
            var claims = new[]
            {
                new Claim("sub", email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Authentication:AccessResetPasswordExpires"]));
            var token = new JwtSecurityToken(claims: claims, signingCredentials: creds, expires: expiry);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiry);
        }

        private string GenerateAccessToken(Guid userId)
        {
            var claims = new[]
            {
                new Claim("sub", userId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims, signingCredentials: creds, expires: DateTime.UtcNow.AddDays(7));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private (string token, DateTime expiry) GenerateRefreshToken()
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            var token = $"refresh_{salt}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var expiry = DateTime.UtcNow.AddDays(30);
            return (token, expiry);
        }

        public async Task<bool> IsUserExist(Guid user_id)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.id == user_id);
            if(user is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<int> modifyPasswordAsync(ModifyPasswordDto dto)
        {
            // validate whether or not user exists
            var user = await _context.users.Include(t => t.user_password).FirstOrDefaultAsync(t => t.id == dto.id);
            if(user is null)
            {
                return -1;
            }

            // validate whether or not old password is correct
            var isCorrect = BCrypt.Net.BCrypt.Verify(dto.current_pwd,user.user_password.password_hash);
            if (!isCorrect)
            {
                return -2;
            }
            user.user_password.password_hash = BCrypt.Net.BCrypt.HashPassword(dto.new_pwd);
            int result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<int> modifyAccountDetailAsync(UsersDto dto)
        {
            var user = await _context.users.FindAsync(dto.id);

            // validate if new email exists
            var userEmail = await _context.users.FirstOrDefaultAsync(t => t.email == dto.email);

            if(userEmail is null || (userEmail is not null && userEmail.id == dto.id))
            {
                user.username = dto.username;
                user.email = dto.email;

                int resultNum = await _context.SaveChangesAsync();

                return resultNum;
            }
            else
            {
                return -1;
            }
        }

        public async Task<int> modifyUserInfoAsync(UsersDto dto)
        {
            var user = await _context.users.FindAsync(dto.id);
            user.bio = dto.bio;
            user.firstname = dto.firstname;
            user.lastname = dto.lastname;

            int resultNum = await _context.SaveChangesAsync();

            return resultNum;
        }

        public async Task<(bool success, string token, string userName)> IsEmailValidAsync(string email)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.email == email);
            if(user is null)
            {
                return (false, "User is not found.", "");
            }
            string? userName = user!.username;

            var (token, expiry) = GenerateTokenByEmail(email);
            user.reset_token = token;
            user.reset_token_expiry = expiry;

            await _context.SaveChangesAsync();
            return (true, token, userName);
        }

        public async Task<(object result, int statusCode)> RegisterUserAsync(RegisterDto dto)
        {
            Guid user_id = Guid.NewGuid();

            var user = new Users { 
                id = user_id,
                username = dto.username,
                email = dto.email,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                roles = "candidate"
            };

            var user_password = new UserPassword {
                user_id = user_id,
                password_hash = BCrypt.Net.BCrypt.HashPassword(dto.password_hash),
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };
            _context.user_password.Add(user_password);
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return (new { 
                code= 0,
                message= "register sucessfully"
            }, 200);
        }

        public async Task<bool> IsExistEmail(string email)
        {
            var user = await _context.users.FirstOrDefaultAsync(n => n.email == email);
            return user != null;
        }

        public async Task<(object result, int statusCode)> LoginUserAsync(LoginDto dto)
        {
            var user = await _context.users.Include(u => u.user_password).FirstOrDefaultAsync(u => u.email == dto.email);
            
            if(user is null || !BCrypt.Net.BCrypt.Verify(dto.password, user.user_password.password_hash))
            {
                return (new { code = 1, message = "Invalid username or password" }, 401);
            }

            var access = GenerateAccessToken(user.id);
            if (dto.remember_me)
            {
                (user.refresh_token, user.refresh_token_expiry) = GenerateRefreshToken();
                await _context.SaveChangesAsync();
            }

            return (new
            {
                code = 0,
                message = "login successfully",
                data = new {
                    access_token = access,
                    refresh_token = user.refresh_token,
                    user = new
                    {
                        id = user.id,
                        username = user.username,
                        email = user.email,
                        avatar_url = user.avatar_url,
                        roles = user.roles
                    }
                }
            }, 200);
        }

        public async Task<(object result, int statusCode)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.refresh_token == refreshToken);
            if(user is null)
            {
                return (new { code = 1, message = "Invalid refresh token." }, 401);
            }
            if(user.refresh_token_expiry is null || user.refresh_token_expiry < DateTime.UtcNow)
            {
                return (new { code = 1, message = "Refresh token has been expired. Please log in again." }, 401);
            }

            var access_token = GenerateAccessToken(user.id);
            // 如果 refresh_token 快过期
            if((user.refresh_token_expiry.Value - DateTime.UtcNow).TotalDays < 7)
            {
                (user.refresh_token, user.refresh_token_expiry) = GenerateRefreshToken();
                await _context.SaveChangesAsync();
            }
            return (new { code = 0, access_token= access_token, refresh_token= user.refresh_token }, 200);
        }

        public async Task<Users> GetUserInfoAsync(Guid user_id)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.id == user_id);
        }

        public async Task<List<PreferredCategories>> GetUserPreferredCategories(Guid user_id)
        {
            return await _context.preferred_categories.Where(p => p.user_id == user_id).ToListAsync();
        }
    }
}
