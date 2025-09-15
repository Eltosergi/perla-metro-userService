using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    public class LoginUserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}