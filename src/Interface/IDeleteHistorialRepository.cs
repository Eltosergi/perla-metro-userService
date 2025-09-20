using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Helpers;

namespace perla_metro_user.src.Interface
{
    public interface IDeleteHistorialRepository
    {
        Task<ResultHelper<IEnumerable<DeleteHistorialDTO>>> GetAllDeleteHistorial();
    }
}