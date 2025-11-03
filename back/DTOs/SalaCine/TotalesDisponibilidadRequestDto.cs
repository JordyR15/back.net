namespace back.DTOs.SalaCine;

public class TotalesDisponibilidadRequestDto
{
    public string Fecha { get; set; } = string.Empty; // yyyy-MM-dd, si vacío usar hoy
}
