using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.DTOs
{
    // DTO que representa una entrada en el historial de eliminaciones.
    // Contiene:
    // - DeletedUser: Información del usuario que fue eliminado (UserDTO).
    // - ActionBy: Información del usuario que realizó la eliminación (UserDTO).
    // - DeletedAt: Fecha y hora en que se realizó la eliminación.
    public class DeleteHistorialDTO
    {
        public required UserDTO DeletedUser { get; set; }
        public required UserDTO ActionBy { get; set; }
        public required DateTime DeletedAt { get; set; }
    }
}