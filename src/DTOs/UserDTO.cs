using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    // DTO que representa la información básica de un usuario.
    // Contiene:
    // - Id: Identificador único del usuario.
    // - Name: Nombre del usuario.
    // - LastName: Apellido del usuario.
    // - Email: Correo electrónico del usuario.
    // - Role: Rol del usuario (por ejemplo, Admin, User).
    // - IsActive: Indica si el usuario está activo.
    // - CreatedAt: Fecha de creación del usuario.
    // Usado en varios endpoints para retornar información del usuario sin exponer datos sensibles como la contraseña.
    // Utilizado en AuthenticatedUserDTO y DeleteHistorialDTO.

    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}