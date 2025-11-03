using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Pelicula;

public class UpdatePeliculaRequestDto
{
    [Required(ErrorMessage = "El ID de la película no puede ser nulo")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la película no puede estar vacío")]
    [MinLength(5, ErrorMessage = "El nombre de la película debe tener al menos 5 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Range(30, int.MaxValue, ErrorMessage = "La duración debe ser de al menos 30 minutos")]
    public int Duracion { get; set; }
}

