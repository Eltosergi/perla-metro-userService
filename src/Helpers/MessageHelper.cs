using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.Helpers
{
    public static class MessagesHelper
    {
        public const string InvalidCredentials = "Correo o contraseña inválidos";
        public const string InvalidRole = "El usuario no tiene un rol válido asignado";
        public const string PasswordRequired = "La contraseña y la confirmación son obligatorias";
        public const string PasswordMismatch = "La contraseña y la confirmación no coinciden";
        public const string InvalidDomain = "Solo se permiten correos electrónicos de perlametro.cl";
        public const string EmailInUse = "El correo electrónico ya está en uso";
        public const string UserCreationError = "Error al crear el usuario";
        public const string LoginSuccess = "Login exitoso";
        public const string RegisterSuccess = "Usuario registrado exitosamente";
    }
}