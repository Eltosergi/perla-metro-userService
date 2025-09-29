using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Interface;

namespace perla_metro_user.src.Controllers
{
    // Controlador para manejar operaciones relacionadas con usuarios. Accesible solo para administradores excepto la actualización de usuario. 
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        // Inyección de la interfaz IUserRepository para manejar la lógica de usuarios.
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        // Este endpoint obtiene todos los usuarios, accesible solo para administradores. Soporta paginación y filtrado. 
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers([FromQuery] QueryParams queryParams)
        {
            var result = await _userRepository.GetAllUsers(queryParams);
            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
        // Este endpoint obtiene un usuario por su ID, accesible solo para administradores. 
        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _userRepository.GetUserById(userId);
            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
        // Este endpoint elimina un usuario por su ID, accesible solo para administradores. Registra la acción en el historial de eliminaciones. Hace un soft delete cambiando el estado del usuario a inactivo.
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminId))
                return Unauthorized(new { Message = "Usuario no autorizado" });
            var result = await _userRepository.DeleteUser(userId, Guid.Parse(adminId));
            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
        // Este endpoint permite a un usuario autenticado actualizar su propia información. Pide el ID del usuario desde el token JWT.
        [HttpPut("Update")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Usuario no autorizado" });

            var result = await _userRepository.UpdateUser(Guid.Parse(userId), dto);
            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

    }
}