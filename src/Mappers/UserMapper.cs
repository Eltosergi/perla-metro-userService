using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Mappers
{

    // Clase estática que contiene métodos para mapear entre diferentes modelos y DTOs relacionados con usuarios.
    // Incluye métodos para mapear desde RegisterUserDTO a User, desde User a AuthenticatedUserDTO, y desde User a UserDTO.
    // Usado en UserService para transformar datos entre capas de la aplicación.
    
    public class UserMapper
    {
        public static User newUserMapper(RegisterUserDTO registerUserDTO, Guid id) =>
            new User
            {
                Id = id,
                Name = registerUserDTO.Name,
                LastName = registerUserDTO.LastName,
                Email = registerUserDTO.Email,
                UserName = registerUserDTO.Email
            };

        public static AuthenticatedUserDTO authenticatedUserMapper(User user, string role, string token) =>
            new AuthenticatedUserDTO
            {
                user = userToUserDTOMapper(user, role),
                Token = token
            };

        public static UserDTO userToUserDTOMapper(User user, string role) =>
            new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                Role = role
            };


    }
}