using Application_Layer.User.DTOs;
using Application_Layer.User.Interfaces;

namespace Application_Layer.User.Services;

public class UserServices : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _userRepository.GetByEmailAsync(email);
    }
}