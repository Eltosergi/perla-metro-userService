using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using perla_metro_user.src.Interface;

namespace perla_metro_user.src.Controllers
{
    [Route("[controller]")]
    public class DeleteHistorialController : Controller
    {
        private readonly IDeleteHistorialRepository _deleteHistorialRepository;

        public DeleteHistorialController(IDeleteHistorialRepository deleteHistorialRepository)
        {
            _deleteHistorialRepository = deleteHistorialRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDeleteHistorial()
        {
            var result = await _deleteHistorialRepository.GetAllDeleteHistorial();
            return StatusCode(result.StatusCode, result);
            
        }

    }
}