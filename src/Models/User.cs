using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace perla_metro_user.src.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string LastName { get; set; }
    }
}