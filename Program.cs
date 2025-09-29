using perla_metro_user.src.Data;
using perla_metro_user.src.Interface;
using perla_metro_user.src.Models;
using perla_metro_user.src.Repository;
using perla_metro_user.src.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using perla_metro_user.src.Data.Seeders;

// Programa principal para configurar y ejecutar la aplicación web.
// Configura servicios, middleware, autenticación, autorización y se asegura de que la base de datos esté actualizada y poblada con datos iniciales.

var builder = WebApplication.CreateBuilder(args); // Crea un constructor de aplicación web con los argumentos proporcionados. 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Obtiene la cadena de conexión a la base de datos desde la configuración.

builder.Services.AddControllers(); // Agrega servicios de controladores al contenedor de servicios.
builder.Services.AddAuthorization(); // Agrega servicios de autorización al contenedor de servicios.


builder.Services.AddScoped<ITokenService, TokenService>(); // Registra el servicio de generación de tokens JWT con alcance por solicitud.
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Registra el repositorio de usuarios con alcance por solicitud.
builder.Services.AddScoped<IDeleteHistorialRepository, DeleteHistorialRepository>(); // Registra el repositorio de eliminación de historial con alcance por solicitud.
// Configura Identity con opciones de contraseña y usa ApplicationDBContext para almacenar usuarios y roles. 
builder.Services.AddIdentity<User, Role>(options => // Configuración de opciones de contraseña para Identity. 
{
    options.Password.RequireDigit = true;               // La contraseña debe contener al menos un dígito.
    options.Password.RequireLowercase = true;   // La contraseña debe contener al menos una letra minúscula.
    options.Password.RequireUppercase = true;  // La contraseña debe contener al menos una letra mayúscula.
    options.Password.RequireNonAlphanumeric = true; // La contraseña debe contener al menos un carácter no alfanumérico.
    options.Password.RequiredLength = 8; // La contraseña debe tener una longitud mínima de 8 caracteres.
    options.Password.RequiredUniqueChars = 1; // La contraseña debe contener al menos 1 carácter único.
}).AddEntityFrameworkStores<ApplicationDBContext>() // Usa ApplicationDBContext para almacenar usuarios y roles.
.AddDefaultTokenProviders();// Agrega proveedores de tokens predeterminados para operaciones como restablecimiento de contraseña. 
// Configura la autenticación JWT con parámetros de validación del token.
builder.Services.AddAuthentication(options => // Configura el esquema de autenticación predeterminado para usar JWT Bearer. 
{// Configura el esquema de autenticación predeterminado para usar JWT Bearer. 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Establece el esquema de autenticación predeterminado en JWT Bearer.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Establece el esquema de desafío predeterminado en JWT Bearer. 
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Establece el esquema predeterminado en JWT Bearer.
}).AddJwtBearer(options => // Configura las opciones específicas para la autenticación JWT Bearer. 
{ // Configura las opciones específicas para la autenticación JWT Bearer. 
    options.TokenValidationParameters = new TokenValidationParameters // Define los parámetros de validación del token JWT. 
    {
        ValidateIssuer = true, // Habilita la validación del emisor del token.
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Establece el emisor válido desde la configuración.
        ValidateAudience = true, // Habilita la validación de la audiencia del token.
        ValidAudience = builder.Configuration["Jwt:Audience"], // Establece la audiencia válida desde la configuración.
        ValidateIssuerSigningKey = true, // Habilita la validación de la clave de firma del emisor.
        ValidateLifetime = true, // Habilita la validación del tiempo de vida del token.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SignInKey"]!)), // Establece la clave de firma del emisor usando una clave simétrica desde la configuración.
        RoleClaimType = ClaimTypes.Role // Especifica el tipo de reclamo que representa el rol del usuario.
    };
});

// Configura el contexto de la base de datos para usar PostgreSQL con la cadena de conexión proporcionada. 
builder.Services.AddDbContext<ApplicationDBContext>(options => // Configura el contexto de la base de datos para usar PostgreSQL con la cadena de conexión proporcionada.
options.UseNpgsql(connectionString)); // Usa PostgreSQL como proveedor de base de datos con la cadena de conexión especificada.
 
builder.Services.AddTransient<UserSeeder>(); // Registra el seeder de usuarios con un tiempo de vida transitorio. 

builder.Services.AddControllers().AddJsonOptions(options => // Configura las opciones de serialización JSON para ignorar ciclos de referencia. 
{ // Configura las opciones de serialización JSON para ignorar ciclos de referencia.
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignora ciclos de referencia durante la serialización JSON para evitar errores.
});// Configura las opciones de serialización JSON para ignorar ciclos de referencia.

var app = builder.Build(); // Construye la aplicación web con la configuración especificada.
app.UseAuthentication(); // Agrega el middleware de autenticación al pipeline de solicitudes.
app.UseAuthorization(); // Agrega el middleware de autorización al pipeline de solicitudes.
app.MapControllers(); // Mapea las rutas de los controladores a los endpoints.

// Asegura que la base de datos esté actualizada y ejecuta el seeder para poblarla con datos iniciales. 
await using (var scope = app.Services.CreateAsyncScope()) // Crea un alcance de servicio asincrónico para obtener servicios necesarios.
{ 
    var services = scope.ServiceProvider; // Obtiene el proveedor de servicios del alcance creado.
 
    var dbContext = services.GetRequiredService<ApplicationDBContext>(); // Obtiene una instancia de ApplicationDBContext desde el proveedor de servicios.
    await dbContext.Database.MigrateAsync(); // Aplica cualquier migración pendiente a la base de datos para asegurar que esté actualizada.

    var seeder = services.GetRequiredService<UserSeeder>(); // Obtiene una instancia de UserSeeder desde el proveedor de servicios.
    await seeder.SeedAsync(); // Ejecuta el método SeedAsync para poblar la base de datos con datos iniciales.
}


app.UseHttpsRedirection(); // Agrega middleware para redirigir solicitudes HTTP a HTTPS.

app.Run(); // Ejecuta la aplicación web.

// Fin del programa principal.