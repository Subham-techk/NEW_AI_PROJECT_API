using Application_Layer.Common;
using Application_Layer.User.DTOs;
using Application_Layer.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NEW_AI_PROJECT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Email is required.",
                    Data = null,
                    StatusCode = 400
                });
            }

            var user = await _userService.GetByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse<UserDto>
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = user,
                StatusCode = 200
            });
        }
    }
}
