using System.ComponentModel.DataAnnotations;

namespace back.DTOs.SalaCine;

public class FiltrarSalasPorEstadoRequestDto
{
    [Required(ErrorMessage = "El estado no puede estar vacío")]
    public string Estado { get; set; } = string.Empty;
}
