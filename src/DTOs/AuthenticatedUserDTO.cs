using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    // DTO que representa un usuario autenticado junto con su token JWT.
    // Incluye la información del usuario y el token para acceder a recursos protegidos.
    // usdo en los endpoints de login y registro.
    // Contiene:
    // - user: Información del usuario autenticado (UserDTO).
    // - Token: Token JWT para autenticación.
    // Usado en los endpoints de login y registro.
    public class AuthenticatedUserDTO
    {
        public required UserDTO user { get; set; }

        public required string Token { get; set; } = string.Empty;
    }
}