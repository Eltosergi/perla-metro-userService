using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Helpers;

namespace perla_metro_user.src.Interface
{
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