using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Pelicula;

public class CreatePeliculaRequestDto
{
    [Required(ErrorMessage = "El nombre de la película no puede estar vacío")]
    [MinLength(5, ErrorMessage = "El nombre de la película debe tener al menos 5 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La duración no puede ser nula")]
    [Range(30, int.MaxValue, ErrorMessage = "La duración debe ser de al menos 30 minutos")]
    public int Duracion { get; set; }
}

