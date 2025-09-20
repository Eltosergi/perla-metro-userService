using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using perla_metro_user.src.Data;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Helpers;
using perla_metro_user.src.Interface;
using perla_metro_user.src.Mappers;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Repository
{
    public class DeleteHistorialRepository : IDeleteHistorialRepository
    {
        private readonly ApplicationDBContext _context;

        private readonly UserManager<User> _userManager;

        public DeleteHistorialRepository(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ResultHelper<IEnumerable<DeleteHistorialDTO>>> GetAllDeleteHistorial()
        {
            var deleteHistorials = await _context.DeleteHistorials
                .Include(h => h.User)
                .Include(h => h.Admin)
                .ToListAsync();

            if (deleteHistorials == null || !deleteHistorials.Any())
            {
                return ResultHelper<IEnumerable<DeleteHistorialDTO>>.Fail(
                    "No hay historial de eliminaciones", 404
                );
            }

            var resultList = deleteHistorials.Select(historial => new DeleteHistorialDTO
            {
                DeletedUser = UserMapper.userToUserDTOMapper(historial.User, "User"),
                ActionBy = UserMapper.userToUserDTOMapper(historial.Admin, "Admin"),
                DeletedAt = historial.Date
            }).ToList();

            return ResultHelper<IEnumerable<DeleteHistorialDTO>>.Success(
                resultList, "Historial de eliminaciones obtenido con éxito"
            );
        }

        
    }
}