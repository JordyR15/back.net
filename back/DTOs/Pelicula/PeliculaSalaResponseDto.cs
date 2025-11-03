namespace back.DTOs.Pelicula;

public class PeliculaSalaResponseDto
{
    public int IdPelicula { get; set; }
    public string NombrePelicula { get; set; } = string.Empty;
    public int? Duracion { get; set; }
    public DateTime? FechaPublicacion { get; set; }
    public int IdSala { get; set; }
    public string NombreSala { get; set; } = string.Empty;
}

