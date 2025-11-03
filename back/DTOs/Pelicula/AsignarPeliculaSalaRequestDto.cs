using System.ComponentModel.DataAnnotations;

namespace back.DTOs.Pelicula;

public class AsignarPeliculaSalaRequestDto
{
    [Required(ErrorMessage = "El ID de la película no puede ser nulo")]
    public int PeliculaId { get; set; }

    [Required(ErrorMessage = "El ID de la sala de cine no puede ser nulo")]
    public int SalaCineId { get; set; }

    [Required(ErrorMessage = "La fecha de publicación no puede ser nula")]
    public DateTime FechaPublicacion { get; set; }

    [Required(ErrorMessage = "La fecha de fin no puede ser nula")]
    public DateTime FechaFin { get; set; }
}

