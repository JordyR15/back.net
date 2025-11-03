using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Pelicula;

public class BuscarPorFechaRequestDto
{
    [Required(ErrorMessage = "La fecha de publicación no puede ser nula")]
    public DateTime FechaPublicacion { get; set; }
}

