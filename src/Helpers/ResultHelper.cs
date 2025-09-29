using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;

namespace perla_metro_user.src.Helpers
{
    // Clase genérica para manejar resultados de operaciones con éxito o fallo.
    // Contiene:
    // - IsSuccess: Indica si la operación fue exitosa.
    // - StatusCode: Código de estado HTTP asociado al resultado.
    // - Message: Mensaje descriptivo del resultado.
    // - Data: Datos resultantes de la operación (de tipo genérico T).
    // Usado en servicios y controladores para estandarizar respuestas.
    public class ResultHelper<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        internal static ResultHelper<T> Fail(string v, int statusCode = 400)
        {
            return new ResultHelper<T> { IsSuccess = false, Message = v, StatusCode = statusCode };
        }

        internal static ResultHelper<T> Success(T data, string message = "")
        {
            return new ResultHelper<T> { IsSuccess = true, Data = data, StatusCode = 200, Message = message };
        }
    }
}