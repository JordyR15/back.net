using back.Data;
using back.DTOs.Pelicula;
using back.Entities;
using back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace back.Services;

public class PeliculaService : IPeliculaService
{
    private readonly ApplicationDbContext _context;

    public PeliculaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Pelicula> CreatePeliculaAsync(CreatePeliculaRequestDto requestDto)
    {
        var pelicula = new Pelicula
        {
            Nombre = requestDto.Nombre,
            Duracion = requestDto.Duracion
        };

        _context.Peliculas.Add(pelicula);
        await _context.SaveChangesAsync();
        return pelicula;
    }

    public async Task<List<Pelicula>> GetAllPeliculasAsync()
    {
        // Llamada al procedimiento almacenado
        return await _context.Peliculas.FromSqlRaw("SELECT * FROM sp_get_all_peliculas()").ToListAsync();
    }

    public async Task<Pelicula> UpdatePeliculaAsync(UpdatePeliculaRequestDto requestDto)
    {
        var pelicula = await _context.Peliculas.FindAsync(requestDto.Id);
        if (pelicula == null)
        {
            throw new ResourceNotFoundException($"No se encontró la película con el ID: {requestDto.Id}");
        }

        pelicula.Nombre = requestDto.Nombre;
        pelicula.Duracion = requestDto.Duracion;

        await _context.SaveChangesAsync();
        return pelicula;
    }

    public async Task DeletePeliculaAsync(DeletePeliculaRequestDto requestDto)
    {
        var pelicula = await _context.Peliculas.FindAsync(requestDto.Id);
        if (pelicula == null)
        {
            throw new ResourceNotFoundException($"No se encontró la película con el ID: {requestDto.Id}");
        }

        pelicula.Activo = false;
        await _context.SaveChangesAsync();
    }

    public async Task<List<PeliculaSalaResponseDto>> BuscarPeliculaPorNombreYSalaAsync(BuscarPeliculaPorNombreYSalaRequestDto requestDto)
    {
        var query = _context.PeliculaSalacines
            .Include(ps => ps.Pelicula)
            .Include(ps => ps.SalaCine)
            .Where(ps => ps.Pelicula.Activo && ps.SalaCine.Activo);

        if (!string.IsNullOrWhiteSpace(requestDto.NombrePelicula))
        {
            var nombreLower = requestDto.NombrePelicula.ToLower();
            query = query.Where(ps => ps.Pelicula.Nombre.ToLower().Contains(nombreLower));
        }

        if (requestDto.IdSala != 0)
        {
            query = query.Where(ps => ps.SalaCineId == requestDto.IdSala);
        }

        var resultados = await query.ToListAsync();

        return resultados.Select(asignacion => new PeliculaSalaResponseDto
        {
            IdPelicula = asignacion.Pelicula.Id,
            NombrePelicula = asignacion.Pelicula.Nombre,
            Duracion = asignacion.Pelicula.Duracion,
            FechaPublicacion = asignacion.FechaPublicacion,
            IdSala = asignacion.SalaCine.Id,
            NombreSala = asignacion.SalaCine.Nombre
        }).ToList();
    }

    public async Task<PeliculaSalacine> AsignarPeliculaASalaAsync(AsignarPeliculaSalaRequestDto requestDto)
    {
        var pelicula = await _context.Peliculas.FindAsync(requestDto.PeliculaId);
        if (pelicula == null)
        {
            throw new ResourceNotFoundException($"No se encontró la película con el ID: {requestDto.PeliculaId}");
        }

        var salaCine = await _context.SalaCines.FindAsync(requestDto.SalaCineId);
        if (salaCine == null)
        {
            throw new ResourceNotFoundException($"No se encontró la sala de cine con el ID: {requestDto.SalaCineId}");
        }

        for (var date = requestDto.FechaPublicacion.Date; date <= requestDto.FechaFin.Date; date = date.AddDays(1))
        {
            var moviesCount = await _context.PeliculaSalacines
                .CountAsync(ps => ps.SalaCineId == requestDto.SalaCineId &&
                                ps.FechaPublicacion <= date &&
                                ps.FechaFin >= date);

            if (moviesCount >= 6)
            {
                throw new InvalidOperationException($"La sala de cine ya tiene 6 o más películas asignadas el {date.ToShortDateString()}.");
            }
        }

        var asignacion = new PeliculaSalacine
        {
            PeliculaId = pelicula.Id,
            SalaCineId = salaCine.Id,
            FechaPublicacion = requestDto.FechaPublicacion,
            FechaFin = requestDto.FechaFin
        };

        _context.PeliculaSalacines.Add(asignacion);
        await _context.SaveChangesAsync();
        
        await _context.Entry(asignacion).Reference(ps => ps.Pelicula).LoadAsync();
        await _context.Entry(asignacion).Reference(ps => ps.SalaCine).LoadAsync();
        
        return asignacion;
    }

    public async Task<PeliculasPorFechaResponseDto> BuscarPorFechaPublicacionAsync(BuscarPorFechaRequestDto requestDto)
    {
        var asignaciones = await _context.PeliculaSalacines
            .Include(ps => ps.Pelicula)
            .Where(ps => ps.FechaPublicacion.HasValue && 
                        ps.FechaPublicacion.Value.Date == requestDto.FechaPublicacion.Date)
            .ToListAsync();

        var peliculas = asignaciones
            .Select(ps => ps.Pelicula)
            .Distinct()
            .Select(p => new PeliculaBasicResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Duracion = p.Duracion
            })
            .ToList();

        return new PeliculasPorFechaResponseDto
        {
            Cantidad = peliculas.Count,
            Peliculas = peliculas
        };
    }

    public async Task<List<PeliculaSalaResponseDto>> BuscarPorNombreAsync(BuscarPorNombreRequestDto requestDto)
    {
        var nombre = (requestDto.Nombre ?? string.Empty).Trim().ToLower();
        if (string.IsNullOrEmpty(nombre))
        {
            return new List<PeliculaSalaResponseDto>();
        }

        var peliculaEncontrada = await _context.Peliculas.FirstOrDefaultAsync(p => p.Nombre.ToLower().Contains(nombre) && p.Activo);
        if (peliculaEncontrada == null)
        {
            return new List<PeliculaSalaResponseDto>();
        }

        var query = _context.PeliculaSalacines
            .Include(ps => ps.Pelicula)
            .Include(ps => ps.SalaCine)
            .Where(ps => ps.Pelicula.Activo && ps.SalaCine.Activo && ps.Pelicula.Nombre.ToLower().Contains(nombre));

        var resultados = await query.ToListAsync();

        if (!resultados.Any())
        {
            throw new ResourceNotFoundException($"La película '{requestDto.Nombre}' no está asignada a ninguna sala.");
        }

        return resultados.Select(asignacion => new PeliculaSalaResponseDto
        {
            IdPelicula = asignacion.Pelicula.Id,
            NombrePelicula = asignacion.Pelicula.Nombre,
            Duracion = asignacion.Pelicula.Duracion,
            FechaPublicacion = asignacion.FechaPublicacion,
            IdSala = asignacion.SalaCine.Id,
            NombreSala = asignacion.SalaCine.Nombre
        }).ToList();
    }
}
