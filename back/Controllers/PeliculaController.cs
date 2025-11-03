using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.DTOs.Pelicula;
using back.Entities;
using back.Services;

namespace back.Controllers;

[ApiController]
[Route("api/peliculas")]
[Authorize]
public class PeliculaController : ControllerBase
{
    private readonly IPeliculaService _peliculaService;

    public PeliculaController(IPeliculaService peliculaService)
    {
        _peliculaService = peliculaService;
    }

    [HttpPost]
    public async Task<ActionResult<Dictionary<string, object>>> CreatePelicula([FromBody] CreatePeliculaRequestDto requestDto)
    {
        var nuevaPelicula = await _peliculaService.CreatePeliculaAsync(requestDto);
        var response = new Dictionary<string, object>
        {
            { "message", "Película creada exitosamente." },
            { "pelicula", nuevaPelicula }
        };
        return StatusCode(201, response);
    }

    [HttpPost("list")]
    public async Task<ActionResult<List<Pelicula>>> GetAllPeliculas()
    {
        var peliculas = await _peliculaService.GetAllPeliculasAsync();
        return Ok(peliculas);
    }

    [HttpPost("update")]
    public async Task<ActionResult<Dictionary<string, object>>> UpdatePelicula([FromBody] UpdatePeliculaRequestDto requestDto)
    {
        var peliculaActualizada = await _peliculaService.UpdatePeliculaAsync(requestDto);
        var response = new Dictionary<string, object>
        {
            { "message", "Película actualizada exitosamente." },
            { "pelicula", peliculaActualizada }
        };
        return Ok(response);
    }

    [HttpPost("delete")]
    public async Task<ActionResult<Dictionary<string, string>>> DeletePelicula([FromBody] DeletePeliculaRequestDto requestDto)
    {
        await _peliculaService.DeletePeliculaAsync(requestDto);
        var response = new Dictionary<string, string>
        {
            { "message", "Película eliminada exitosamente." }
        };
        return Ok(response);
    }

    [HttpPost("buscar-por-fecha")]
    public async Task<ActionResult<PeliculasPorFechaResponseDto>> BuscarPorFecha([FromBody] BuscarPorFechaRequestDto requestDto)
    {
        var response = await _peliculaService.BuscarPorFechaPublicacionAsync(requestDto);
        return Ok(response);
    }

    [HttpPost("buscar-por-nombre")]
    public async Task<ActionResult<List<PeliculaSalaResponseDto>>> BuscarPorNombre([FromBody] BuscarPorNombreRequestDto requestDto)
    {
        var peliculas = await _peliculaService.BuscarPorNombreAsync(requestDto);
        return Ok(peliculas);
    }
}
