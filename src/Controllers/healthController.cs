using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace perla_metro_user.src.Controllers
{
    [Route("[controller]")]
    public class healthController : Controller
    {
        private readonly ILogger<healthController> _logger;

        public healthController(ILogger<healthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Service is running");
        }
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            throw new Exception("This is a test exception for error handling.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error500()
        {
            return Problem("An unexpected error occurred. Please try again later.", statusCode: 500);
        }

        [HttpGet("exception")]
        public IActionResult Exception()
        {
            throw new InvalidOperationException("This is a test exception.");
        }

    }
}