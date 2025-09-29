using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    // DTO para manejar los datos necesarios para el login de un usuario.
    // Contiene:
    // - Email: Correo electrónico del usuario.
    // - Password: Contraseña del usuario.
    // Usado en el endpoint de login.
    // Retorna un token JWT si las credenciales son correctas.
    public class LoginUserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}