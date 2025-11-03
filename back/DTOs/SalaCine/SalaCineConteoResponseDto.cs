namespace back.DTOs.SalaCine;

public class SalaCineConteoResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int PeliculasAsignadas { get; set; }
}
