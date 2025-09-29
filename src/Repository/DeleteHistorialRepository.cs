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

// Repositorio que maneja las operaciones relacionadas con el historial de eliminaciones de usuarios.
// Implementa IDeleteHistorialRepository y define el método para obtener todas las entradas del historial
// de eliminaciones. Utiliza ApplicationDBContext para interactuar con la base de datos y UserManager para
// manejar operaciones relacionadas con usuarios.

namespace perla_metro_user.src.Repository // Repositorio que maneja las operaciones relacionadas con el historial de eliminaciones de usuarios. 
{
    public class DeleteHistorialRepository : IDeleteHistorialRepository // Implementa IDeleteHistorialRepository y define el método para obtener todas las entradas del historial de eliminaciones. Utiliza ApplicationDBContext para interactuar con la base de datos y UserManager para manejar operaciones relacionadas con usuarios.
    {
        // Inyección de dependencias para ApplicationDBContext y UserManager.
        private readonly ApplicationDBContext _context;
        
        // UserManager para manejar operaciones relacionadas con usuarios.
        private readonly UserManager<User> _userManager;

        // Constructor que recibe ApplicationDBContext y UserManager a través de inyección de dependencias.
        // Asigna los parámetros a las variables privadas.
        public DeleteHistorialRepository(ApplicationDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Método asincrónico para obtener todas las entradas del historial de eliminaciones.
        // Incluye las entidades relacionadas de usuario y administrador.
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