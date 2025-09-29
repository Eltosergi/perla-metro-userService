using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.Helpers.Request
{
    // Clase para manejar los parámetros de paginación en las solicitudes.
    // Contiene:
    // - PageNumber: Número de página (predeterminado 1).
    // - PageSize: Tamaño de página (predeterminado 20, máximo 20).
    // Usado en controladores para recibir parámetros de paginación.
    public class PaginationParams
    {
        private const int MaxPageSize = 20;

        public int PageNumber { get; set; } = 1;
        private int _pageSize = 20;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}