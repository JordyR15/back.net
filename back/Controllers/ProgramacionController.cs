using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.DTOs.Pelicula;
using back.Services;

namespace back.Controllers;

[ApiController]
[Route("api/programacion")]
[Authorize]
public class ProgramacionController : ControllerBase
{
    private readonly IPeliculaService _peliculaService;

    public ProgramacionController(IPeliculaService peliculaService)
    {
        _peliculaService = peliculaService;
    }

    // /api/programacion/asignar
    [HttpPost("asignar")]
    public async Task<ActionResult<Dictionary<string, object>>> Asignar([FromBody] AsignarPeliculaSalaRequestDto requestDto)
    {
        var asignacion = await _peliculaService.AsignarPeliculaASalaAsync(requestDto);

        var asignacionDto = new PeliculaSalaResponseDto
        {
            IdPelicula = asignacion.Pelicula.Id,
            NombrePelicula = asignacion.Pelicula.Nombre,
            Duracion = asignacion.Pelicula.Duracion,
            FechaPublicacion = asignacion.FechaPublicacion,
            IdSala = asignacion.SalaCine.Id,
            NombreSala = asignacion.SalaCine.Nombre
        };

        var response = new Dictionary<string, object>
        {
            { "message", "Película asignada a la sala exitosamente." },
            { "asignacion", asignacionDto }
        };
        return StatusCode(201, response);
    }
}
