using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace perla_metro_user.src.Models
{
    // Modelo que representa un rol en el sistema de identidad.
    // Hereda de IdentityRole con clave primaria de tipo GUID.
    // El constructor asigna un nuevo GUID al Id del rol.
    // Usado para gestionar roles de usuario en la aplicación.
    // Contiene:
    // - Id: Identificador único del rol (GUID).
    // - Name: Nombre del rol (heredado de IdentityRole).
    // - NormalizedName: Nombre normalizado del rol (heredado de IdentityRole).
    // - ConcurrencyStamp: Marca de concurrencia para el rol (heredado de IdentityRole).
    // Usado en la gestión de roles y permisos.
    // Ejemplos de roles: "Admin", "User", etc.

    public class Role : IdentityRole<Guid>// IdentityRole requiere que el tipo de clave primaria sea especificado.
    {
        public Role()
        {
            Id = Guid.NewGuid();
        }

    }
}