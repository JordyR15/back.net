namespace back.DTOs.SalaCine;

public class TotalesDisponibilidadResponseDto
{
    public int TotalSalas { get; set; }
    public int TotalDisponibles { get; set; }
    public int TotalConAsignaciones { get; set; } // 3 a 5
    public int TotalNoDisponibles { get; set; }   // >5
    public string Fecha { get; set; } = string.Empty;
}
