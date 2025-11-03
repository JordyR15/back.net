using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace back.Services;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly long _expiration;
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret");
        _expiration = long.Parse(configuration["Jwt:Expiration"] ?? "86400000");
    }

    public string GenerateToken(string username, List<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = GetSigningKey();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMilliseconds(_expiration),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetSigningKey();

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public bool IsTokenValid(string token)
    {
        try
        {
            var principal = GetPrincipalFromToken(token);
            return principal != null;
        }
        catch
        {
            return false;
        }
    }

    private SymmetricSecurityKey GetSigningKey()
    {
        var keyBytes = Convert.FromBase64String(_secretKey);
        return new SymmetricSecurityKey(keyBytes);
    }

    public string ExtractUsername(string token)
    {
        var principal = GetPrincipalFromToken(token);
        return principal?.Identity?.Name ?? string.Empty;
    }
}

