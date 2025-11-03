using System.Security.Claims;

namespace back.Services;

public interface IJwtService
{
    string GenerateToken(string username, List<string> roles);
    ClaimsPrincipal? GetPrincipalFromToken(string token);
    bool IsTokenValid(string token);
}

