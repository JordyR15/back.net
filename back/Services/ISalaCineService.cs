using back.DTOs.SalaCine;
using back.Entities;

namespace back.Services;

public interface ISalaCineService
{
    Task<SalaCine> CreateSalaCineAsync(CreateSalaCineRequestDto requestDto);
    Task<List<SalaCineConteoResponseDto>> GetAllSalasAsync();
    Task<SalaCine> UpdateSalaCineAsync(UpdateSalaCineRequestDto requestDto);
    Task DeleteSalaCineAsync(DeleteSalaCineRequestDto requestDto);
    Task<EstadoSalaResponseDto> ObtenerEstadoSalaAsync(EstadoSalaRequestDto requestDto);
    Task<List<back.DTOs.Pelicula.PeliculaBasicResponseDto>> ObtenerEstrenosPorSalaAsync(EstrenosSalaRequestDto requestDto);
    Task<TotalesDisponibilidadResponseDto> ObtenerTotalesDisponibilidadAsync(TotalesDisponibilidadRequestDto requestDto);
    Task<List<SalaCineConteoResponseDto>> FiltrarSalasPorEstadoAsync(FiltrarSalasPorEstadoRequestDto requestDto);
}
