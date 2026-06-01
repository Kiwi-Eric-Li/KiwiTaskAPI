using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServiceRepository _authRepository;
        private readonly IMailService _mail;
        private readonly IMapper _mapper;


        
        public AuthController(IAuthServiceRepository authRepository, IMailService mail, IMapper mapper)
        {
            _authRepository = authRepository;
            _mail = mail;
            _mapper = mapper;
        }

        [HttpGet("user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            // get current user's id
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _authRepository.GetUserInfoAsync(Guid.Parse(userId));
            if(user is null)
            {
                return Ok(new
                {
                    code = 1,
                    message = "user not found"
                });
            }
            var userDto = _mapper.Map<UsersDto>(user);
            return Ok(new
            {
                code = 0,
                data = userDto
            });
        }

        [HttpGet("preferred-category")]
        [Authorize]
        public async Task<IActionResult> GetUserPreferredCategory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<PreferredCategories> list = await _authRepository.GetUserPreferredCategories(Guid.Parse(userId));
            return Ok(new
            {
                code = 0,
                data = list
            });
        }

        [HttpPut("preferred-category")]
        [Authorize]
        public async Task<IActionResult> ModifyUserPreferredCategory([FromBody] List<PreferredCategories> preferredCategoriesList)
        {
            var count = await _authRepository.ModifyUserPreferredCategories(preferredCategoriesList);
            return Ok(new
            {
                code = 0,
                count = count
            });
        }

        [HttpPut("notification-settings")]
        [Authorize]
        public async Task<IActionResult> SetNotificationSettings([FromBody] Dictionary<string, int> data)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authRepository.ModifyNotificationSettings(Guid.Parse(userId), data);

            return Ok(new
            {
                code = 0,
                data = result
            });
        }

        [HttpGet("notification-settings")]
        [Authorize]
        public async Task<IActionResult> GetNotificationSettings([FromQuery] Guid user_id)
        {
            var notification_settings = await _authRepository.GetNotificationSettings(user_id);
            if(notification_settings is not null)
            {
                return Ok(new
                {
                    code = 0,
                    data = notification_settings
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1
                });
            }
            
        }

        [HttpPost("forget-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] EmailDto dto)
        {
            var (ok, msg, userName) = await _authRepository.IsEmailValidAsync(dto.Email);
            if (ok)
            {
                Console.WriteLine(ok);
                Console.WriteLine(msg);
                Console.WriteLine(userName);
            }
            else
            {
               
            }
            return Ok();
        }

        [HttpPut("modify-password")]
        [Authorize]
        public async Task<IActionResult> ModifyPassword([FromBody] ModifyPasswordDto dto)
        {

            int result = await _authRepository.modifyPasswordAsync(dto);

            if(result == -1)
            {
                return Ok(new
                {
                    code = -1,
                    message = "the user is not found"
                });
            }
            else if(result == -2)
            {
                return Ok(new
                {
                    code = -2,
                    message = "the old password is not correct"
                });
            }
            else
            {
                return Ok(new
                {
                    code = 0,
                    message = "password is modified successfully"
                });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // 在注册之前，先验证邮箱是否存在
            var flag = await _authRepository.IsExistEmail(dto.email);
            if (flag)
            {
                return BadRequest(new {code = 1, message = "Email already exists."});
            }
            
            var (result, statusCode) = await _authRepository.RegisterUserAsync(dto);
            if(statusCode == 200)
            {
                // 发送邮件
                await _mail.SendWelcomeEmailAsync(dto.username, dto.email);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.email) || string.IsNullOrWhiteSpace(dto.password))
            {
                return BadRequest(new {code = 1, message = "Missing required fields."});
            }

            var (result, statusCode) = await _authRepository.LoginUserAsync(dto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.refresh_token))
            {
                return BadRequest(new { code = 1, message = "Missing refresh token." });
            }
            var (result, statusCode) = await _authRepository.RefreshTokenAsync(dto.refresh_token);
            return Ok(result);
        }

        [HttpPut("user-info")]
        [Authorize]
        public async Task<IActionResult> ModifyUserInfo([FromBody] UsersDto dto, [FromQuery] string flag)
        {
            // 1. validate if user_id is existed.
            var result = await _authRepository.IsUserExist(dto.id);
            if (!result)
            {
                return Ok(new
                {
                    code = 1,
                    message = "user not found"
                });
            }
            // 2. modify full name and bio
            var resultNum = 0;
            if(flag == "profile")
            {
                resultNum = await _authRepository.modifyUserInfoAsync(dto);
            }
            else
            {
                resultNum = await _authRepository.modifyAccountDetailAsync(dto);
                if(resultNum == -1)
                {
                    return Ok(new
                    {
                        code = 1,
                        message = "this email exists"
                    });
                }
            }

            if (resultNum > 0)
            {
                return Ok(new
                {
                    code = 0,
                    message = "update successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    message = "update failed"
                });
            }

        }
    }
}
