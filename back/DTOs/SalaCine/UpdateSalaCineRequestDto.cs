using System.ComponentModel.DataAnnotations;

namespace back.DTOs.SalaCine;

public class UpdateSalaCineRequestDto
{
    [Required(ErrorMessage = "El ID de la sala no puede ser nulo")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la sala no puede estar vacío")]
    public string Nombre { get; set; } = string.Empty;
}

