using System.ComponentModel.DataAnnotations;

namespace back.DTOs.SalaCine;

public class DeleteSalaCineRequestDto
{
    [Required(ErrorMessage = "El ID de la sala no puede ser nulo")]
    public int Id { get; set; }
}

