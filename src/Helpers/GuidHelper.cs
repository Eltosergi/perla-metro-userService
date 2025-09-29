using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using perla_metro_user.src.Data;

// Clase auxiliar para generar GUIDs únicos para usuarios. Previene colisiones en IDs de usuarios.
// Utiliza el contexto de la base de datos para verificar la unicidad del GUID generado.
namespace perla_metro_user.src.Helpers
{
    // Clase auxiliar para generar GUIDs únicos para usuarios. Previene colisiones en IDs de usuarios.
    // Utiliza el contexto de la base de datos para verificar la unicidad del GUID generado.
    public class GuidHelper
    {
        // Método estático para generar un GUID único para un usuario.
        // Verifica en la base de datos que el GUID no exista ya.
        public static async Task<Guid> GenerateUniqueUserIdAsync(ApplicationDBContext context) // Método estático para generar un GUID único para un usuario. Verifica en la base de datos que el GUID no exista ya.
        {
            Guid newId; // Nuevo GUID generado
            bool exists; // Indicador de existencia en la base de datos 
            // Bucle para generar un nuevo GUID hasta encontrar uno que no exista en la base de datos.
            do // Bucle para generar un nuevo GUID hasta encontrar uno que no exista en la base de datos. 
            {// Genera un nuevo GUID y verifica su existencia en la base de datos.
                newId = Guid.NewGuid();// Genera un nuevo GUID y verifica su existencia en la base de datos. por cada iteración. guarda el nuevo GUID en newId.
                // Verifica si el GUID ya existe en la base de datos.
                exists = await context.Users.AnyAsync(u => u.Id == newId); // Verifica si el GUID ya existe en la base de datos. Si existe, el bucle continúa.
            }// Continúa el bucle hasta encontrar un GUID único.
            while (exists);// Continúa el bucle hasta encontrar un GUID único. 

            return newId; // Retorna el GUID único generado. 
        }
    }
}

