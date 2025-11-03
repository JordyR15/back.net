using back.DTOs.Role;

namespace back.Services;

public interface IRolService
{
    Task<List<RoleResponseDto>> GetAllRolesAsync(RoleListRequestDto? requestDto);
}

