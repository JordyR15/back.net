using back.DTOs.Pelicula;

namespace back.DTOs.SalaCine;

public class EstadoSalaResponseDto
{
    public string Mensaje { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public List<PeliculaBasicResponseDto> Peliculas { get; set; } = new();
}
