using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Mappers
{
    public class UserMapper
    {
        public static User newUserMapper(RegisterUserDTO registerUserDTO) =>
            new User
            {
                Name = registerUserDTO.Name,
                LastName = registerUserDTO.LastName,
                Email = registerUserDTO.Email,
                UserName = registerUserDTO.Email
            };

        public static AuthenticatedUserDTO authenticatedUserMapper(User user, string role, string token) =>
            new AuthenticatedUserDTO
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Role = role,
                Token = token
            };
    }
}