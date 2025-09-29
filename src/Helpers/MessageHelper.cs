using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace perla_metro_user.src.Helpers
{

    // Clase estática que contiene mensajes constantes usados en la aplicación.
    // Estos mensajes son utilizados para proporcionar retroalimentación al usuario

    public static class MessagesHelper
    {
        public const string InvalidCredentials = "Correo o contraseña inválidos";
        public const string UserDeactivated = "El usuario está eliminado";
        public const string InvalidRole = "El usuario no tiene un rol válido asignado";
        public const string PasswordRequired = "La contraseña y la confirmación son obligatorias";
        public const string PasswordMismatch = "La contraseña y la confirmación no coinciden";
        public const string WeakPassword = "La contraseña no cumple con los requisitos de seguridad";
        public const string InvalidDomain = "Solo se permiten correos electrónicos de perlametro.cl";
        public const string EmailInUse = "El correo electrónico ya está en uso";
        public const string UserCreationError = "Error al crear el usuario";
        public const string LoginSuccess = "Login exitoso";
        public const string RegisterSuccess = "Usuario registrado exitosamente";
        public const string UsersFetched = "Usuarios obtenidos exitosamente";
        public const string NoUsersFound = "No se encontraron usuarios registrados";
        public const string UserDeleted = "Usuario eliminado exitosamente";
        public const string UserNotFound = "Usuario no encontrado";

        public const string UserUpdated = "Usuario actualizado exitosamente";
        public const string UserUpdateError = "Error al actualizar el usuario";
        public const string SelfDeletionNotAllowed = "No puedes eliminar tu propia cuenta";

    }
}