using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;

namespace perla_metro_user.src.Helpers
{
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