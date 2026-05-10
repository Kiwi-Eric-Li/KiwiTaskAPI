using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServiceRepository _authRepository;
        private readonly IMailService _mail;

        
        public AuthController(IAuthServiceRepository authRepository, IMailService mail)
        {
            _authRepository = authRepository;
            _mail = mail;
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
        

    }
}
