using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    // DTO para manejar los datos necesarios para actualizar la información de un usuario.
    // Todos los campos son opcionales para permitir actualizaciones parciales.
    // Contiene:
    // - Name: Nombre del usuario.
    // - LastName: Apellido del usuario.
    // - Email: Correo electrónico del usuario.
    // - Password: Contraseña del usuario.
    // - ConfirmPassword: Confirmación de la contraseña.
    // Usado en el endpoint de actualización de usuario.
    
    public class UpdateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}