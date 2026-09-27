using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Auth.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(string userId, string email, string fullName);
    }
}
