using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Helpers;

namespace perla_metro_user.src.Interface
{
    // Interfaz para el repositorio que maneja el historial de eliminaciones de usuarios.
    // Define un método para obtener todas las entradas del historial de eliminaciones.
    public interface IDeleteHistorialRepository
    {
        Task<ResultHelper<IEnumerable<DeleteHistorialDTO>>> GetAllDeleteHistorial();
    }
}