using Application_Layer.Auth.DTOs;
using Application_Layer.Auth.Interfaces;
using Application_Layer.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NEW_AI_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Registration failed.",
                    StatusCode = 400
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Registration successful.",
                StatusCode = 200
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    StatusCode = 401
                });
            }

            return Ok(new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = result,
                StatusCode = 200
            });
        }
    }
}
