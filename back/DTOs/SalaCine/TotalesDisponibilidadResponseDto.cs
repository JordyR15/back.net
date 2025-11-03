namespace back.DTOs.SalaCine;

public class TotalesDisponibilidadResponseDto
{
    public int TotalSalas { get; set; }
    public int TotalDisponibles { get; set; }
    public int TotalConAsignaciones { get; set; }
    public int TotalNoDisponibles { get; set; } 
    public string Fecha { get; set; } = string.Empty;
}
