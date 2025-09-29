using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using perla_metro_user.src.Interface;
using perla_metro_user.src.Models;

// Servicio para manejar la generación de tokens JWT.
// Implementa la interfaz ITokenService.


namespace perla_metro_user.src.Service
{
    // Clase que implementa la generación de tokens JWT para autenticación.
    // Utiliza la configuración proporcionada para crear tokens firmados con HMAC SHA-512.
    // El token incluye reclamos como email, nombre, ID y rol del usuario.
    // Usado en UserService para generar tokens al autenticar usuarios.
    // Contiene un método para generar el token basado en la información del usuario y su rol
    
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            var signingKey = _config["Jwt:SignInKey"] ?? throw new ArgumentNullException("Key not found");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

        }

        public string GenerateToken(User user, string Role)
        {
            var claims = new List<Claim>
            {
               new(JwtRegisteredClaimNames.Email, user.Email!),
               new(JwtRegisteredClaimNames.GivenName, user.Name),
               new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
               new(ClaimTypes.Role, Role),
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}