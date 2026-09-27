using Application_Layer.User.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.User.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto?> GetByEmailAsync(string email);
    }
}
