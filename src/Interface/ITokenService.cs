using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user, string Role);
    }
}