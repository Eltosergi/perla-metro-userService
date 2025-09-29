using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace perla_metro_user.src.Models
{
    // Modelo que representa un usuario en el sistema.
    // Hereda de IdentityUser con clave primaria de tipo GUID.
    // Contiene:
    // - Name: Nombre del usuario.
    // - LastName: Apellido del usuario.
    // - IsActive: Indica si el usuario está activo (predeterminado true).
    // - CreatedAt: Fecha de creación del usuario (predeterminado a la fecha actual).
    // Usado para gestionar usuarios en la aplicación.
    // Ejemplos de usuarios: Administradores, usuarios regulares, etc.

    public class User : IdentityUser<Guid> // IdentityUser requiere que el tipo de clave primaria sea especificado.
    {

        [Required]
        public required string Name { get; set; }
        [Required]
        public required string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    }
}