using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace perla_metro_user.src.Models
{
    public class DeleteHistorial
    {
        [Key]
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required Guid UserId { get; set; }
        public required User User { get; set; }
        [Required]
        public required Guid AdminId { get; set; }
        public required User Admin { get; set; }
        [Required]
        public required DateTime Date { get; set; }
    }
}