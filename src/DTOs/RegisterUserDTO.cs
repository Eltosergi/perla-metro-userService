using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    // DTO para manejar los datos necesarios para el registro de un nuevo usuario.
    // Contiene:
    // - Name: Nombre del usuario.
    // - LastName: Apellido del usuario.
    // - Email: Correo electrónico del usuario.
    // - Password: Contraseña del usuario.
    // - ConfirmPassword: Confirmación de la contraseña.
    // Usado en el endpoint de registro.
    public class RegisterUserDTO
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}