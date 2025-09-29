using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Interface
{
    // Interfaz para el servicio que maneja la generación de tokens JWT.
    // Define un método para generar un token basado en la información del usuario y su rol.
    
    public interface ITokenService
    {
        string GenerateToken(User user, string Role);
    }
}