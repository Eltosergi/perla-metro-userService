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
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            var result = await _userRepository.Login(loginUserDTO);

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            var result = await _userRepository.Register(registerUserDTO);

            return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
        }
    }
}