using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Interface;


namespace perla_metro_user.src.Controllers
{
    // Controlador para manejar la autenticación y el registro de usuarios. 
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        // Inyección de la interfaz IUserRepository para manejar la lógica de autenticación y registro.
        private readonly IUserRepository _userRepository;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        // Este endpoint maneja el login de usuarios y retorna un token JWT si las credenciales son correctas.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            var result = await _userRepository.Login(loginUserDTO);

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
        // Este endpoint maneja el registro de nuevos usuarios. retorna un token JWT si el registro es exitoso.
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            var result = await _userRepository.Register(registerUserDTO);

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}