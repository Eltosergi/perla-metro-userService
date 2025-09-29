using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Helpers;

namespace perla_metro_user.src.Interface
{
    // Interfaz para el repositorio que maneja las operaciones relacionadas con los usuarios.
    // Define métodos para registrar, autenticar, obtener, actualizar y eliminar usuarios.
    // Usado en UserService para implementar la lógica de negocio relacionada con usuarios.
    // Contiene métodos asincrónicos que retornan ResultHelper con los resultados de las operaciones
    
    public interface IUserRepository
    {
        Task<ResultHelper<AuthenticatedUserDTO>> Register(RegisterUserDTO registerUserDTO);
        Task<ResultHelper<AuthenticatedUserDTO>> Login(LoginUserDTO loginUserDTO);
        Task<ResultHelper<IEnumerable<UserDTO>>> GetAllUsers(QueryParams queryParams);
        Task<ResultHelper<UserDTO>> GetUserById(Guid userId);
        Task<ResultHelper<UserDTO>> DeleteUser(Guid userId, Guid adminId);
        Task<ResultHelper<UserDTO>> UpdateUser(Guid userId, UpdateUserDTO updateUserDTO);
    }
}