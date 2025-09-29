using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using perla_metro_user.src.Data;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Helpers;
using perla_metro_user.src.Helpers.Request;
using perla_metro_user.src.Interface;
using perla_metro_user.src.Mappers;
using perla_metro_user.src.Models;

namespace perla_metro_user.src.Repository
{
    // Repositorio que maneja las operaciones relacionadas con los usuarios.
    // Implementa IUserRepository y define métodos para registrar, autenticar, obtener, actualizar y
    // eliminar usuarios. Utiliza ApplicationDBContext para interactuar con la base de datos,
    // UserManager para manejar operaciones relacionadas con usuarios, y ITokenService para generar tokens JWT
    // Usado en UserService para implementar la lógica de negocio relacionada con usuarios.
    // Contiene métodos asincrónicos que retornan ResultHelper con los resultados de las operaciones

    public class UserRepository : IUserRepository
    {
        // Inyección de dependencias para ApplicationDBContext, ITokenService y UserManager.
        private readonly ApplicationDBContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        // Constructor que recibe ApplicationDBContext, ITokenService y UserManager a través de inyección de dependencias.
        // Asigna los parámetros a las variables privadas.

        public UserRepository(ApplicationDBContext context, ITokenService tokenService, UserManager<User> userManager)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        // Método asincrónico para obtener un usuario por su ID.
        // Usa UserManager para buscar el usuario y obtener su rol.

        public async Task<ResultHelper<UserDTO>> GetUserById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResultHelper<UserDTO>.Fail(MessagesHelper.UserNotFound, 404);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "User";

            var token = _tokenService.GenerateToken(user, roleName);

            return ResultHelper<UserDTO>.Success(
                UserMapper.userToUserDTOMapper(user, roleName),
                "Usuario obtenido exitosamente"
            );
        }
        // Método asincrónico para obtener todos los usuarios con rol "User".
        // Soporta paginación y filtrado por nombre completo y estado activo/inactivo.
        public async Task<ResultHelper<IEnumerable<UserDTO>>> GetAllUsers(QueryParams queryParams)
        {

            var usersInRole = await _userManager.GetUsersInRoleAsync("User");
            IEnumerable<User> filteredUsers = usersInRole;


            if (!string.IsNullOrWhiteSpace(queryParams.Fullname))
            {
                var fullnameLower = queryParams.Fullname.ToLower();
                filteredUsers = filteredUsers.Where(u =>
                    (u.Name + " " + u.LastName).ToLower().Contains(fullnameLower));
            }


            if (queryParams.IsActive.HasValue)
            {
                filteredUsers = filteredUsers.Where(u => u.IsActive == queryParams.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(queryParams.Email))
            {
                var emailLower = queryParams.Email.ToLower();
                filteredUsers = filteredUsers.Where(u => u.Email.ToLower().Contains(emailLower));
            }


            var pagedList = PagedList<User>.ToPagedList(
                filteredUsers, queryParams.PageNumber, queryParams.PageSize
            );

            if (pagedList == null || !pagedList.Any())
            {
                return ResultHelper<IEnumerable<UserDTO>>.Fail(MessagesHelper.NoUsersFound, 404);
            }

            var userDTOs = pagedList.Select(user => UserMapper.userToUserDTOMapper(user, "User")).ToList();

            return ResultHelper<IEnumerable<UserDTO>>.Success(userDTOs, MessagesHelper.UsersFetched);
        }

        // Método asincrónico para autenticar a un usuario y generar un token JWT.
        // Usa UserManager para validar las credenciales y obtener el rol del usuario.

        public async Task<ResultHelper<AuthenticatedUserDTO>> Login(LoginUserDTO loginUserDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDTO.Email);

            if (user == null)
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.InvalidCredentials, 401);
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginUserDTO.Password);
            if (!passwordValid)
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.InvalidCredentials, 401);
            }

            if (!user.IsActive)
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.UserDeactivated, 403);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var validRoles = new[] { "Admin", "User" };
            var roleName = roles.FirstOrDefault(r => validRoles.Contains(r));

            if (string.IsNullOrEmpty(roleName))
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.InvalidRole, 400);
            }

            var token = _tokenService.GenerateToken(user, roleName);

            return BuildAuthenticatedUserResult(user, roleName, token, MessagesHelper.LoginSuccess);
        }

        // Método asincrónico para registrar un nuevo usuario.
        // Valida el correo electrónico, la contraseña y verifica si el correo ya está en uso.
        // Usa UserManager para crear el usuario y asignarle el rol "User".

        public async Task<ResultHelper<AuthenticatedUserDTO>> Register(RegisterUserDTO registerUserDTO)
        {
            if (string.IsNullOrWhiteSpace(registerUserDTO.Password) || string.IsNullOrWhiteSpace(registerUserDTO.ConfirmPassword))
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.PasswordRequired, 400);
            }

            if (registerUserDTO.Password != registerUserDTO.ConfirmPassword)
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.PasswordMismatch, 400);
            }
            if (!Regex.IsMatch(registerUserDTO.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.WeakPassword, 400);
            }

            if (!Regex.IsMatch(registerUserDTO.Email, @"^[^@\s]+@perlametro\.cl$", RegexOptions.IgnoreCase))
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.InvalidDomain, 400);
            }

            var existingUser = await _userManager.FindByEmailAsync(registerUserDTO.Email);
            if (existingUser != null)
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.EmailInUse, 409);
            }

            var user = UserMapper.newUserMapper(registerUserDTO, await GuidHelper.GenerateUniqueUserIdAsync(_context));
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResultHelper<AuthenticatedUserDTO>.Fail($"{MessagesHelper.UserCreationError}: {errors}", 400);
            }

            // Asignar rol en BD
            await _userManager.AddToRoleAsync(user, "User");

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "User"; // fallback

            var token = _tokenService.GenerateToken(user, roleName);

            return BuildAuthenticatedUserResult(user, roleName, token, MessagesHelper.RegisterSuccess);
        }

        // Método asincrónico para eliminar (desactivar) un usuario.
        // Verifica que el usuario no se esté eliminando a sí mismo.
        // Usa UserManager para actualizar el estado del usuario y registra la eliminación en DeleteHistorial.

        public async Task<ResultHelper<UserDTO>> DeleteUser(Guid userId, Guid adminId)
        {
            if (userId == adminId)
            {
                return ResultHelper<UserDTO>.Fail(MessagesHelper.SelfDeletionNotAllowed, 400);
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ResultHelper<UserDTO>.Fail(MessagesHelper.UserNotFound, 404);
            }

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResultHelper<UserDTO>.Fail($"{MessagesHelper.UserCreationError}: {errors}", 400);
            }

            var adminUser = await _userManager.FindByIdAsync(adminId.ToString());
            if (adminUser == null)
            {
                return ResultHelper<UserDTO>.Fail(MessagesHelper.UserNotFound, 404);
            }

            var deleteRecord = new DeleteHistorial
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                User = user,
                AdminId = adminId,
                Admin = adminUser,
                Date = DateTime.UtcNow
            };

            _context.DeleteHistorials.Add(deleteRecord);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "User";
            return ResultHelper<UserDTO>.Success(UserMapper.userToUserDTOMapper(user, roleName), MessagesHelper.UserDeleted);
        }

        // Método asincrónico para actualizar la información de un usuario.
        // Permite actualizar el correo electrónico, la contraseña, el nombre y el apellido.
        // Valida el correo electrónico y la contraseña, y verifica si el correo ya está en uso.

        public async Task<ResultHelper<UserDTO>> UpdateUser(Guid userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ResultHelper<UserDTO>.Fail(MessagesHelper.UserNotFound, 404);


            if (!string.IsNullOrWhiteSpace(updateUserDTO.Email) &&
                !updateUserDTO.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (!Regex.IsMatch(updateUserDTO.Email, @"^[^@\s]+@perlametro\.cl$", RegexOptions.IgnoreCase))
                    return ResultHelper<UserDTO>.Fail(MessagesHelper.InvalidDomain, 400);

                var existingUser = await _userManager.FindByEmailAsync(updateUserDTO.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                    return ResultHelper<UserDTO>.Fail(MessagesHelper.EmailInUse, 409);

                user.Email = updateUserDTO.Email;
                user.UserName = updateUserDTO.Email;
            }

            if (!string.IsNullOrWhiteSpace(updateUserDTO.Password) ||
                !string.IsNullOrWhiteSpace(updateUserDTO.ConfirmPassword))
            {
                if (updateUserDTO.Password != updateUserDTO.ConfirmPassword)
                    return ResultHelper<UserDTO>.Fail(MessagesHelper.PasswordMismatch, 400);

                // Usa el PasswordValidator de Identity para coherencia con la configuración global
                var passwordValidation = await _userManager.PasswordValidators.First()
                    .ValidateAsync(_userManager, user, updateUserDTO.Password);
                if (!passwordValidation.Succeeded)
                {
                    var errors = string.Join(", ", passwordValidation.Errors.Select(e => e.Description));
                    return ResultHelper<UserDTO>.Fail($"{MessagesHelper.WeakPassword}: {errors}", 400);
                }

                var removePassword = await _userManager.RemovePasswordAsync(user);
                if (!removePassword.Succeeded)
                    return ResultHelper<UserDTO>.Fail(MessagesHelper.UserUpdateError, 400);

                var addPassword = await _userManager.AddPasswordAsync(user, updateUserDTO.Password);
                if (!addPassword.Succeeded)
                {
                    var errors = string.Join(", ", addPassword.Errors.Select(e => e.Description));
                    return ResultHelper<UserDTO>.Fail($"{MessagesHelper.UserUpdateError}: {errors}", 400);
                }
            }

            if (!string.IsNullOrWhiteSpace(updateUserDTO.Name))
            {
                user.Name = updateUserDTO.Name;
            }
            if (!string.IsNullOrWhiteSpace(updateUserDTO.LastName))
            {
                user.LastName = updateUserDTO.LastName;
            }


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResultHelper<UserDTO>.Fail($"{MessagesHelper.UserUpdateError}: {errors}", 400);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "User";

            return ResultHelper<UserDTO>.Success(
                UserMapper.userToUserDTOMapper(user, roleName),
                MessagesHelper.UserUpdated);
        }

        // Método privado para construir un ResultHelper con los datos del usuario autenticado.

        private ResultHelper<AuthenticatedUserDTO> BuildAuthenticatedUserResult(User user, string roleName, string token, string message)
        {
            return ResultHelper<AuthenticatedUserDTO>.Success(
                UserMapper.authenticatedUserMapper(user, roleName, token),
                message
            );
        }



    }

}
