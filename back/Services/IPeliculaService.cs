using back.DTOs.Pelicula;
using back.Entities;

namespace back.Services;

public interface IPeliculaService
{
    Task<Pelicula> CreatePeliculaAsync(CreatePeliculaRequestDto requestDto);
    Task<List<Pelicula>> GetAllPeliculasAsync();
    Task<Pelicula> UpdatePeliculaAsync(UpdatePeliculaRequestDto requestDto);
    Task DeletePeliculaAsync(DeletePeliculaRequestDto requestDto);
    Task<List<PeliculaSalaResponseDto>> BuscarPeliculaPorNombreYSalaAsync(BuscarPeliculaPorNombreYSalaRequestDto requestDto);
    Task<PeliculaSalacine> AsignarPeliculaASalaAsync(AsignarPeliculaSalaRequestDto requestDto);
    Task<PeliculasPorFechaResponseDto> BuscarPorFechaPublicacionAsync(BuscarPorFechaRequestDto requestDto);
    Task<List<PeliculaSalaResponseDto>> BuscarPorNombreAsync(BuscarPorNombreRequestDto requestDto);
}
