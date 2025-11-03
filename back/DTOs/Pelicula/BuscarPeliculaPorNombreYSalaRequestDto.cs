using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Pelicula;

public class BuscarPeliculaPorNombreYSalaRequestDto
{
    [Required(ErrorMessage = "El nombre de la película no puede estar vacío")]
    public string NombrePelicula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ID de la sala no puede ser nulo")]
    public int IdSala { get; set; }
}

