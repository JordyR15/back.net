using System.ComponentModel.DataAnnotations;

namespace back.DTOs.User;

public class CreateUserRequestDto
{
    [Required(ErrorMessage = "El nombre de usuario no puede estar vacío")]
    [MinLength(4, ErrorMessage = "El nombre de usuario debe tener al menos 4 caracteres")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña no puede estar vacía")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre no puede estar vacío")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido no puede estar vacío")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email no puede estar vacío")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ID del rol no puede ser nulo")]
    public int RoleId { get; set; }
}

