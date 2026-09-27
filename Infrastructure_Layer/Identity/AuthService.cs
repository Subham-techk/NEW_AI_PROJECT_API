using Application_Layer.Auth.DTOs;
using Application_Layer.Auth.Interfaces;

using Infrastructure_Layer.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager; 
            _jwtTokenService = jwtTokenService;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return false;
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return false;
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user,request.Password);

            return result.Succeeded;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            var user =await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return null;
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user,request.Password);

            if (!passwordValid)
            {
                return null;
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user.Id,user.Email!,user.FullName);

            return new LoginResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(2),
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!
            };
        }
    }
}
