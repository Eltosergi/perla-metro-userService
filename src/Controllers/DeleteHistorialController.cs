using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using perla_metro_user.src.Interface;

namespace perla_metro_user.src.Controllers
{
    // Controlador para manejar el historial de eliminaciones. Accesible solo para administradores. 
    [Route("[controller]")]
    public class DeleteHistorialController : Controller
    {
        // Inyección de la interfaz IDeleteHistorialRepository para manejar la lógica del historial de eliminaciones. 
        private readonly IDeleteHistorialRepository _deleteHistorialRepository;

        public DeleteHistorialController(IDeleteHistorialRepository deleteHistorialRepository)
        {
            _deleteHistorialRepository = deleteHistorialRepository;
        }

        // Este endpoint obtiene todo el historial de eliminaciones, accesible solo para administradores.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDeleteHistorial()
        {
            var result = await _deleteHistorialRepository.GetAllDeleteHistorial();
            return StatusCode(result.StatusCode, result);

        }

    }
}