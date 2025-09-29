using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Data
{
    // Contexto de la base de datos que extiende IdentityDbContext para manejar la autenticación y autorización. 
    // Incluye DbSet para el historial de eliminaciones y configura roles predeterminados.
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : IdentityDbContext<User, Role, Guid>(options) // Hereda de IdentityDbContext para manejar usuarios y roles con claves GUID.
    {
        // DbSet para manejar el historial de eliminaciones.
        public DbSet<DeleteHistorial> DeleteHistorials { get; set; }
        // Configuración inicial del modelo, incluyendo la inserción de roles predeterminados. 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de roles predeterminados: Admin y User.
            List<Role> roles = new List<Role>
            {
                new Role
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            // Inserción de roles predeterminados en la base de datos.
            builder.Entity<Role>().HasData(roles);
        }
    }
}