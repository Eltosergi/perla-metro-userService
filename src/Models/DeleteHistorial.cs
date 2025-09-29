using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace perla_metro_user.src.Models
{
    // Modelo que representa una entrada en el historial de eliminaciones de usuarios.
    // Contiene:
    // - Id: Identificador único de la entrada (GUID).
    // - UserId: Identificador del usuario que fue eliminado (GUID).
    // - User: Referencia al usuario que fue eliminado (navegación).
    // - AdminId: Identificador del administrador que realizó la eliminación (GUID).
    // - Admin: Referencia al administrador que realizó la eliminación (navegación).
    // - Date: Fecha y hora en que se realizó la eliminación.
    // Usado para registrar y auditar las eliminaciones de usuarios en el sistema.
    
    public class DeleteHistorial
    {
        [Key]
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required Guid UserId { get; set; }
        public required User User { get; set; }
        [Required]
        public required Guid AdminId { get; set; }
        public required User Admin { get; set; }
        [Required]
        public required DateTime Date { get; set; }
    }
}