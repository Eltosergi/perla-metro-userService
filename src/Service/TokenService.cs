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

namespace perla_metro_user.src.Service
{
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