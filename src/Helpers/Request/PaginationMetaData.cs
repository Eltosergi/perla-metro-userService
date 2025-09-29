using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.Helpers.Request
{
    // Clase que representa los metadatos de paginación para respuestas paginadas.
    // Contiene información sobre el total de elementos, tamaño de página, página actual y total
    // de páginas.
    // Usado en la clase PagedList para proporcionar detalles de paginación en las respuestas.
    public class PaginationMetaData
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}