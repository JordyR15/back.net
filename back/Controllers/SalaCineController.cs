using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.DTOs.SalaCine;
using back.Entities;
using back.Services;

namespace back.Controllers;

[ApiController]
[Route("api/salas")]
[Authorize]
public class SalaCineController : ControllerBase
{
    private readonly ISalaCineService _salaCineService;

    public SalaCineController(ISalaCineService salaCineService)
    {
        _salaCineService = salaCineService;
    }

    // /api/salas (para agregar)
    [HttpPost]
    public async Task<ActionResult<Dictionary<string, object>>> CreateSalaCine([FromBody] CreateSalaCineRequestDto requestDto)
    {
        var nuevaSala = await _salaCineService.CreateSalaCineAsync(requestDto);
        var response = new Dictionary<string, object>
        {
            { "message", "Sala de cine creada exitosamente." },
            { "sala", nuevaSala }
        };
        return StatusCode(201, response);
    }

    // /api/salas/list (getTodasSalas)
    [HttpPost("list")]
    public async Task<ActionResult<List<SalaCineConteoResponseDto>>> GetAllSalas()
    {
        var salas = await _salaCineService.GetAllSalasAsync();
        return Ok(salas);
    }

    // /api/salas/update
    [HttpPost("update")]
    public async Task<ActionResult<Dictionary<string, object>>> UpdateSalaCine([FromBody] UpdateSalaCineRequestDto requestDto)
    {
        var salaActualizada = await _salaCineService.UpdateSalaCineAsync(requestDto);
        var response = new Dictionary<string, object>
        {
            { "message", "Sala de cine actualizada exitosamente." },
            { "sala", salaActualizada }
        };
        return Ok(response);
    }

    // /api/salas/delete
    [HttpPost("delete")]
    public async Task<ActionResult<Dictionary<string, string>>> DeleteSalaCine([FromBody] DeleteSalaCineRequestDto requestDto)
    {
        await _salaCineService.DeleteSalaCineAsync(requestDto);
        var response = new Dictionary<string, string>
        {
            { "message", "Sala de cine eliminada exitosamente." }
        };
        return Ok(response);
    }

    // /api/salas/estado-sala
    [HttpPost("estado-sala")]
    public async Task<ActionResult<EstadoSalaResponseDto>> EstadoSala([FromBody] EstadoSalaRequestDto requestDto)
    {
        var resultado = await _salaCineService.ObtenerEstadoSalaAsync(requestDto);
        if (resultado.Mensaje.StartsWith("Formato de fecha inválido"))
        {
            return BadRequest(new Dictionary<string, string> { { "error", resultado.Mensaje } });
        }
        return Ok(resultado);
    }

    // /api/salas/estrenos-por-sala
    [HttpPost("estrenos-por-sala")]
    public async Task<ActionResult<List<back.DTOs.Pelicula.PeliculaBasicResponseDto>>> EstrenosPorSala([FromBody] EstrenosSalaRequestDto requestDto)
    {
        var peliculas = await _salaCineService.ObtenerEstrenosPorSalaAsync(requestDto);
        return Ok(peliculas);
    }

    // /api/salas/total-disponibles
    [HttpPost("total-disponibles")]
    public async Task<ActionResult<TotalesDisponibilidadResponseDto>> TotalesDisponibles([FromBody] TotalesDisponibilidadRequestDto requestDto)
    {
        var totales = await _salaCineService.ObtenerTotalesDisponibilidadAsync(requestDto);
        return Ok(totales);
    }

    // /api/salas/filtrar-por-estado
    [HttpPost("filtrar-por-estado")]
    public async Task<ActionResult<List<SalaCineConteoResponseDto>>> FiltrarPorEstado([FromBody] FiltrarSalasPorEstadoRequestDto requestDto)
    {
        var salas = await _salaCineService.FiltrarSalasPorEstadoAsync(requestDto);
        return Ok(salas);
    }
}
