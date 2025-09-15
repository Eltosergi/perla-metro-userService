using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using perla_metro_user.src.Data;
using perla_metro_user.src.DTOs;
using perla_metro_user.src.Helpers;
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

            // Validar dominio del correo
            if (!Regex.IsMatch(registerUserDTO.Email, @"^[^@\s]+@perlametro\.cl$", RegexOptions.IgnoreCase))
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.InvalidDomain, 400);
            }

            // Verificar si ya existe
            var existingUser = await _userManager.FindByEmailAsync(registerUserDTO.Email);
            if (existingUser != null)
            {
                return ResultHelper<AuthenticatedUserDTO>.Fail(MessagesHelper.EmailInUse, 409);
            }

            var user = UserMapper.newUserMapper(registerUserDTO);
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

        
        private ResultHelper<AuthenticatedUserDTO> BuildAuthenticatedUserResult(User user, string roleName, string token, string message)
        {
            return ResultHelper<AuthenticatedUserDTO>.Success(
                UserMapper.authenticatedUserMapper(user, roleName, token),
                message
            );
        }
    }

}
