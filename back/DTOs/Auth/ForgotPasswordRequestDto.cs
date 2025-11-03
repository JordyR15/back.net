using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Auth;

public class ForgotPasswordRequestDto
{
    [Required(ErrorMessage = "El email no puede estar vacío")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;
}

