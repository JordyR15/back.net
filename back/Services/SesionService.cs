using back.Data;
using back.DTOs.Auth;
using back.Entities;
using back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace back.Services;

public class SesionService : ISesionService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;

    public SesionService(
        ApplicationDbContext context,
        IJwtService jwtService,
        IPasswordHasher passwordHasher,
        IEmailService emailService)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var user = await _context.Users
            .Include(u => u.Person)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == loginRequestDto.Username && u.Activo);

        if (user == null || !_passwordHasher.VerifyPassword(loginRequestDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        var roles = user.UserRoles
            .Where(ur => ur.Role != null)
            .Select(ur => $"ROLE_{ur.Role!.Name.ToUpper()}")
            .ToList();

        var token = _jwtService.GenerateToken(user.Username, roles);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            Roles = roles
        };
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestDto requestDto)
    {
        var user = await _context.Users
            .Include(u => u.Person)
            .FirstOrDefaultAsync(u => u.Person != null && u.Person.Email == requestDto.Email && u.Activo);

        if (user != null && user.Person != null)
        {
            var token = Guid.NewGuid().ToString();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTimeOffset.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            await _emailService.SendPasswordResetEmailAsync(user.Person.Email, token);
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordRequestDto requestDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.ResetToken == requestDto.Token && u.Activo);

        if (user == null)
        {
            throw new ResourceNotFoundException("Token de reseteo inválido o no encontrado.");
        }

        if (user.ResetTokenExpiry == null || user.ResetTokenExpiry < DateTimeOffset.UtcNow)
        {
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _context.SaveChangesAsync();
            throw new InvalidTokenException("El token de reseteo ha expirado.");
        }

        user.PasswordHash = _passwordHasher.HashPassword(requestDto.NewPassword);
        user.ResetToken = null;
        user.ResetTokenExpiry = null;

        await _context.SaveChangesAsync();
    }
}

