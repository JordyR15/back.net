using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "El nombre de usuario no puede estar vacío")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña no puede estar vacía")]
    public string Password { get; set; } = string.Empty;
}

