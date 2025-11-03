using back.Data;
using back.DTOs.Pelicula;
using back.DTOs.SalaCine;
using back.Entities;
using back.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace back.Services;

public class SalaCineService : ISalaCineService
{
    private readonly ApplicationDbContext _context;

    public SalaCineService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SalaCine> CreateSalaCineAsync(CreateSalaCineRequestDto requestDto)
    {
        var salaCine = new SalaCine
        {
            Nombre = requestDto.Nombre
        };

        _context.SalaCines.Add(salaCine);
        await _context.SaveChangesAsync();
        return salaCine;
    }

    public async Task<List<SalaCineConteoResponseDto>> GetAllSalasAsync()
    {
        var hoy = DateTime.UtcNow.Date;
        var todasLasSalas = await _context.SalaCines.Where(s => s.Activo).ToListAsync();
        var salasConConteo = new List<SalaCineConteoResponseDto>();

        foreach (var sala in todasLasSalas)
        {
            var conteo = await _context.PeliculaSalacines
                .CountAsync(ps => ps.SalaCineId == sala.Id &&
                                ps.FechaPublicacion.HasValue && ps.FechaFin.HasValue &&
                                ps.FechaPublicacion.Value.Date <= hoy &&
                                ps.FechaFin.Value.Date >= hoy);

            salasConConteo.Add(new SalaCineConteoResponseDto { Id = sala.Id, Nombre = sala.Nombre, PeliculasAsignadas = conteo });
        }

        return salasConConteo;
    }

    public async Task<SalaCine> UpdateSalaCineAsync(UpdateSalaCineRequestDto requestDto)
    {
        var sala = await _context.SalaCines.FindAsync(requestDto.Id);
        if (sala == null)
        {
            throw new ResourceNotFoundException($"No se encontró la sala de cine con el ID: {requestDto.Id}");
        }

        sala.Nombre = requestDto.Nombre;
        await _context.SaveChangesAsync();
        return sala;
    }

    public async Task DeleteSalaCineAsync(DeleteSalaCineRequestDto requestDto)
    {
        var sala = await _context.SalaCines.FindAsync(requestDto.Id);
        if (sala == null)
        {
            throw new ResourceNotFoundException($"No se encontró la sala de cine con el ID: {requestDto.Id}");
        }

        sala.Activo = false;
        await _context.SaveChangesAsync();
    }

    public async Task<EstadoSalaResponseDto> ObtenerEstadoSalaAsync(EstadoSalaRequestDto requestDto)
    {
        if (string.IsNullOrWhiteSpace(requestDto.NombreSala))
        {
            return new EstadoSalaResponseDto { Mensaje = "El nombre de la sala no puede estar vacío.", Cantidad = 0, Peliculas = new() };
        }

        if (!DateTime.TryParseExact(requestDto.Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
        {
            return new EstadoSalaResponseDto { Mensaje = "Formato de fecha inválido. Use yyyy-MM-dd.", Cantidad = 0, Peliculas = new() };
        }

        var sala = await _context.SalaCines
            .FirstOrDefaultAsync(s => s.Activo && s.Nombre.ToLower() == requestDto.NombreSala.ToLower());

        if (sala == null)
        {
            return new EstadoSalaResponseDto { Mensaje = "Sala no encontrada.", Cantidad = 0, Peliculas = new() };
        }

        var asignaciones = await _context.PeliculaSalacines
            .Include(ps => ps.Pelicula)
            .Include(ps => ps.SalaCine)
            .Where(ps => ps.SalaCineId == sala.Id
                && ps.Pelicula.Activo
                && ps.SalaCine.Activo
                && ps.FechaPublicacion.HasValue
                && ps.FechaFin.HasValue
                && ps.FechaPublicacion.Value.Date <= fecha.Date
                && ps.FechaFin.Value.Date >= fecha.Date)
            .ToListAsync();

        var peliculas = asignaciones
            .Select(ps => ps.Pelicula)
            .Distinct()
            .Select(p => new PeliculaBasicResponseDto { Id = p.Id, Nombre = p.Nombre, Duracion = p.Duracion })
            .ToList();

        var cantidad = peliculas.Count;
        string mensaje = cantidad switch
        {
            < 3 => "Sala disponible",
            >= 3 and <= 5 => $"Sala con {cantidad} películas asignadas",
            > 5 => "Sala no disponible"
        };

        return new EstadoSalaResponseDto
        {
            Mensaje = mensaje,
            Cantidad = cantidad,
            Peliculas = peliculas
        };
    }

    public async Task<List<PeliculaBasicResponseDto>> ObtenerEstrenosPorSalaAsync(EstrenosSalaRequestDto requestDto)
    {
        if (string.IsNullOrWhiteSpace(requestDto.NombreSala))
        {
            return new List<PeliculaBasicResponseDto>();
        }

        var sala = await _context.SalaCines
            .FirstOrDefaultAsync(s => s.Activo && s.Nombre.ToLower() == requestDto.NombreSala.ToLower());

        if (sala == null)
        {
            return new List<PeliculaBasicResponseDto>();
        }

        var asignaciones = await _context.PeliculaSalacines
            .Include(ps => ps.Pelicula)
            .Where(ps => ps.SalaCineId == sala.Id && ps.Pelicula.Activo)
            .OrderBy(ps => ps.FechaPublicacion)
            .ToListAsync();

        var peliculas = asignaciones
            .Select(ps => ps.Pelicula)
            .Distinct()
            .Select(p => new PeliculaBasicResponseDto { Id = p.Id, Nombre = p.Nombre, Duracion = p.Duracion })
            .ToList();

        return peliculas;
    }

    public async Task<TotalesDisponibilidadResponseDto> ObtenerTotalesDisponibilidadAsync(TotalesDisponibilidadRequestDto requestDto)
    {
        DateTime fecha;
        if (string.IsNullOrWhiteSpace(requestDto.Fecha))
        {
            fecha = DateTime.UtcNow.Date;
        }
        else if (!DateTime.TryParseExact(requestDto.Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
        {
            return new TotalesDisponibilidadResponseDto
            {
                Fecha = requestDto.Fecha,
                TotalSalas = 0,
                TotalDisponibles = 0,
                TotalConAsignaciones = 0,
                TotalNoDisponibles = 0
            };
        }

        var salasActivas = await _context.SalaCines.Where(s => s.Activo).Select(s => new { s.Id }).ToListAsync();
        var salaIds = salasActivas.Select(s => s.Id).ToList();

        var conteos = await _context.PeliculaSalacines
            .Where(ps => salaIds.Contains(ps.SalaCineId)
                && ps.FechaPublicacion.HasValue && ps.FechaFin.HasValue
                && ps.FechaPublicacion.Value.Date <= fecha.Date
                && ps.FechaFin.Value.Date >= fecha.Date
                && ps.Pelicula.Activo
                && ps.SalaCine.Activo)
            .GroupBy(ps => ps.SalaCineId)
            .Select(g => new { SalaId = g.Key, Cantidad = g.Select(x => x.PeliculaId).Distinct().Count() })
            .ToListAsync();

        var conteoPorSala = conteos.ToDictionary(x => x.SalaId, x => x.Cantidad);

        int totalSalas = salaIds.Count;
        int totalDisponibles = salaIds.Count(id => !conteoPorSala.ContainsKey(id) || conteoPorSala[id] < 3);
        int totalConAsignaciones = salaIds.Count(id => conteoPorSala.ContainsKey(id) && conteoPorSala[id] >= 3 && conteoPorSala[id] <= 5);
        int totalNoDisponibles = salaIds.Count(id => conteoPorSala.ContainsKey(id) && conteoPorSala[id] > 5);

        return new TotalesDisponibilidadResponseDto
        {
            Fecha = fecha.ToString("yyyy-MM-dd"),
            TotalSalas = totalSalas,
            TotalDisponibles = totalDisponibles,
            TotalConAsignaciones = totalConAsignaciones,
            TotalNoDisponibles = totalNoDisponibles
        };
    }

    public async Task<List<SalaCineConteoResponseDto>> FiltrarSalasPorEstadoAsync(FiltrarSalasPorEstadoRequestDto requestDto)
    {
        var hoy = DateTime.UtcNow.Date;
        var todasLasSalas = await _context.SalaCines.Where(s => s.Activo).ToListAsync();
        var salasFiltradas = new List<SalaCineConteoResponseDto>();

        foreach (var sala in todasLasSalas)
        {
            var conteo = await _context.PeliculaSalacines
                .CountAsync(ps => ps.SalaCineId == sala.Id &&
                                ps.FechaPublicacion.HasValue && ps.FechaFin.HasValue &&
                                ps.FechaPublicacion.Value.Date <= hoy &&
                                ps.FechaFin.Value.Date >= hoy);

            if (requestDto.Estado.ToLower() == "disponibles" && conteo < 3)
            {
                salasFiltradas.Add(new SalaCineConteoResponseDto { Id = sala.Id, Nombre = sala.Nombre, PeliculasAsignadas = conteo });
            }
            else if (requestDto.Estado.ToLower() == "con_asignaciones" && conteo >= 3 && conteo <= 5)
            {
                salasFiltradas.Add(new SalaCineConteoResponseDto { Id = sala.Id, Nombre = sala.Nombre, PeliculasAsignadas = conteo });
            }
            else if (requestDto.Estado.ToLower() == "no_disponibles" && conteo > 5)
            {
                salasFiltradas.Add(new SalaCineConteoResponseDto { Id = sala.Id, Nombre = sala.Nombre, PeliculasAsignadas = conteo });
            }
        }

        return salasFiltradas;
    }
}
