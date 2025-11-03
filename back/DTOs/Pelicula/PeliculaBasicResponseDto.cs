namespace back.DTOs.Pelicula;

public class PeliculaBasicResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int? Duracion { get; set; }
}
