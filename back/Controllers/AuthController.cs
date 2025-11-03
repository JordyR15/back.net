using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.DTOs.Auth;
using back.Services;

namespace back.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly ISesionService _sesionService;

    public AuthController(ISesionService sesionService)
    {
        _sesionService = sesionService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var response = await _sesionService.LoginAsync(loginRequestDto);
        return Ok(response);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<Dictionary<string, string>>> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
    {
        await _sesionService.ForgotPasswordAsync(requestDto);
        var response = new Dictionary<string, string>
        {
            { "message", "Si su dirección de correo electrónico está en nuestra base de datos, recibirá un enlace para restablecer la contraseña." }
        };
        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<Dictionary<string, string>>> ResetPassword([FromBody] ResetPasswordRequestDto requestDto)
    {
        await _sesionService.ResetPasswordAsync(requestDto);
        var response = new Dictionary<string, string>
        {
            { "message", "Su contraseña ha sido actualizada exitosamente." }
        };
        return Ok(response);
    }
}

