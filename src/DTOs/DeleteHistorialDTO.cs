using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.DTOs
{
    public class DeleteHistorialDTO
    {
        public required UserDTO DeletedUser { get; set; }
        public required UserDTO ActionBy { get; set; }
        public required DateTime DeletedAt { get; set; }
    }
}