using back.Data;
using back.DTOs.Role;
using Microsoft.EntityFrameworkCore;

namespace back.Services;

public class RolService : IRolService
{
    private readonly ApplicationDbContext _context;

    public RolService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleResponseDto>> GetAllRolesAsync(RoleListRequestDto? requestDto)
    {
        var roles = await _context.Roles.ToListAsync();
        return roles.Select(r => new RoleResponseDto
        {
            Id = r.Id,
            Name = r.Name
        }).ToList();
    }
}

