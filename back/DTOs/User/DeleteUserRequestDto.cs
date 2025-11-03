using System.ComponentModel.DataAnnotations;

namespace back.DTOs.User;

public class DeleteUserRequestDto
{
    [Required(ErrorMessage = "El ID del usuario no puede ser nulo")]
    public long Id { get; set; }
}

