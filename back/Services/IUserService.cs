using back.DTOs.User;

namespace back.Services;

public interface IUserService
{
    Task CreateUserAsync(CreateUserRequestDto requestDto);
    Task<List<UserListResponseDto>> ListUsersAsync(ListUserRequestDto? requestDto);
    Task DeleteUserAsync(DeleteUserRequestDto requestDto);
}

