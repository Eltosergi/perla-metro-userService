using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    public class AuthenticatedUserDTO
    {
        public required UserDTO user { get; set; } 

        public required string Token { get; set; } = string.Empty;
    }
}