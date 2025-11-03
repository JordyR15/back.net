using back.DTOs.Auth;

namespace back.Services;

public interface ISesionService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    Task ForgotPasswordAsync(ForgotPasswordRequestDto requestDto);
    Task ResetPasswordAsync(ResetPasswordRequestDto requestDto);
}

