using System.ComponentModel.DataAnnotations;

namespace back.DTOs.SalaCine;

public class BuscarPorNombreSalaRequestDto
{
    [Required(ErrorMessage = "El nombre de la sala no puede estar vacío")]
    public string NombreSala { get; set; } = string.Empty;
}

