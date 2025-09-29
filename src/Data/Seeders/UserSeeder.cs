using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Data.Seeders
{
    // Clase para sembrar datos iniciales de usuarios en la base de datos.
    // En este caso, crea un usuario administrador si no existen usuarios en la base de datos.
    // Usa UserManager para manejar la creación y asignación de roles al usuario.
    // Usado en la inicialización de la aplicación para asegurar que siempre haya un usuario administrador
    // con credenciales predeterminadas.
    // El usuario administrador creado tiene:
    // - UserName: "Admin"
    // - Email: "admin@perlametro.cl"
    // - Password: "Admin@123"
    // - Role: "Admin"
    // Se recomienda cambiar estas credenciales en un entorno de producción.
    // Ejemplo de uso: Llamar a SeedAsync() durante la inicialización de la aplicación.
    // Nota: Asegurarse de que las credenciales predeterminadas sean seguras y se cambien después del primer inicio de sesión.
    // El método SeedAsync es asincrónico y debe ser esperado.
    // Verifica si hay usuarios en la base de datos antes de crear el usuario administrador para
    // evitar duplicados.
    // Requiere inyección de dependencias para ApplicationDBContext y UserManager<User>.
    // El rol "Admin" debe existir previamente en la base de datos.
    // Este seeder es útil para entornos de desarrollo y pruebas.
    public class UserSeeder
    {
        private readonly ApplicationDBContext _context; // Contexto de la base de datos. 
        private readonly UserManager<User> _userManager; // UserManager para manejar operaciones relacionadas con usuarios.
        
        // Constructor que recibe el contexto de la base de datos y el UserManager a través de inyección de dependencias. 
        public UserSeeder(ApplicationDBContext context, UserManager<User> userManager) // Constructor que recibe el contexto de la base de datos y el UserManager a través de inyección de dependencias.
        {
            _context = context; // Asigna el contexto de la base de datos.
            _userManager = userManager; // Asigna el UserManager. 
        }
        // Método asincrónico para sembrar datos iniciales de usuarios.
        // Crea un usuario administrador si no existen usuarios en la base de datos.
        // Usa UserManager para manejar la creación y asignación de roles al usuario.
        // Verifica si hay usuarios en la base de datos antes de crear el usuario administrador para
        // evitar duplicados.
        // Requiere que el rol "Admin" exista previamente en la base de datos.
        public async Task SeedAsync() // Método asincrónico para sembrar datos iniciales de usuarios. Crea un usuario administrador si no existen usuarios en la base de datos. Usa UserManager para manejar la creación y asignación de roles al usuario.
        {
            // Verifica si hay usuarios en la base de datos.
            // Si no hay usuarios, crea un usuario administrador con credenciales predeterminadas.
            // El rol "Admin" debe existir previamente en la base de datos.
            if (!_context.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "admin@perlametro.cl",
                    Name = "Admin",
                    LastName = "User"
                };
                await _userManager.CreateAsync(adminUser, "Admin@123");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}