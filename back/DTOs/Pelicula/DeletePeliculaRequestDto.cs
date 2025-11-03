using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Pelicula;

public class DeletePeliculaRequestDto
{
    [Required(ErrorMessage = "El ID de la película no puede ser nulo")]
    public int Id { get; set; }
}

