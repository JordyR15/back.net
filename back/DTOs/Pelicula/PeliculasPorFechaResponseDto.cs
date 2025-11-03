using back.DTOs.Pelicula;

namespace back.DTOs.Pelicula;

public class PeliculasPorFechaResponseDto
{
    public int Cantidad { get; set; }
    public List<PeliculaBasicResponseDto> Peliculas { get; set; } = new List<PeliculaBasicResponseDto>();
}
