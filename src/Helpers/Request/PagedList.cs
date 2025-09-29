using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace perla_metro_user.src.Helpers.Request
{
    // Clase genérica para manejar listas paginadas.
    // Proporciona metadatos de paginación y métodos para crear listas paginadas desde consultas IQueryable o IEnumerable.
    // Contiene:
    // - Metadata: Información de paginación (total de elementos, tamaño de página, página actual, total de páginas).
    // - ToPagedList: Métodos estáticos para crear una PagedList desde IQueryable o IEnumerable.
    // Usado en endpoints que retornan listas de recursos con paginación.

    public class PagedList<T> : List<T>
    {
        // Constructor que inicializa la lista paginada y sus metadatos.
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Metadata = new PaginationMetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            AddRange(items);
        }
        // Metadatos de paginación.
        public PaginationMetaData Metadata { get; set; }

        // Método estático para crear una PagedList desde una consulta IQueryable.
        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
        // Método estático para crear una PagedList desde una colección IEnumerable.
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
