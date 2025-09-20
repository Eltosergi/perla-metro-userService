using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.DTOs
{
    public class QueryParams
    {
        public string? Fullname { get; set; }
        public bool? IsActive { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        
    }
} 