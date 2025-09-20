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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDBContext context, ITokenService tokenService, UserManager<User> userManager)
        {
            _context = context;
            _tokenService = tokenService;
            _userManager = userManager;
        }

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

            if(!string.IsNullOrWhiteSpace(updateUserDTO.Name))
            {
                user.Name = updateUserDTO.Name;
            }
            if(!string.IsNullOrWhiteSpace(updateUserDTO.LastName))
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


        private ResultHelper<AuthenticatedUserDTO> BuildAuthenticatedUserResult(User user, string roleName, string token, string message)
        {
            return ResultHelper<AuthenticatedUserDTO>.Success(
                UserMapper.authenticatedUserMapper(user, roleName, token),
                message
            );
        }

        
        
    }

}
