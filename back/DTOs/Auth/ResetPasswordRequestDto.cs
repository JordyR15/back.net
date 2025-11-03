using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Auth;

public class ResetPasswordRequestDto
{
    [Required(ErrorMessage = "El token no puede estar vacío")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña no puede estar vacía")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string NewPassword { get; set; } = string.Empty;
}

