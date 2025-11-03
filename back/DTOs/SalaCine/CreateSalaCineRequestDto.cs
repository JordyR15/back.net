using System.ComponentModel.DataAnnotations;

namespace back.DTOs.SalaCine;

public class CreateSalaCineRequestDto
{
    [Required(ErrorMessage = "El nombre de la sala no puede estar vacío")]
    public string Nombre { get; set; } = string.Empty;
}

