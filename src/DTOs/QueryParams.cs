using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    // DTO para manejar parámetros de consulta comunes en paginación y filtrado.
    // Contiene:
    // - Fullname: Filtrar por nombre completo (opcional).
    // - IsActive: Filtrar por estado activo/inactivo (opcional).
    // - PageNumber: Número de página para paginación (predeterminado 1).
    // - PageSize: Tamaño de página para paginación (predeterminado 20).
    public class QueryParams
    {
        public string? Fullname { get; set; }
        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;


    }
} 