using back.Data;
using back.DTOs.User;
using back.Entities;
using back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace back.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task CreateUserAsync(CreateUserRequestDto requestDto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == requestDto.Username))
        {
            throw new DuplicateResourceException($"El nombre de usuario ya está en uso: {requestDto.Username}");
        }

        if (await _context.Persons.AnyAsync(p => p.Email == requestDto.Email))
        {
            throw new DuplicateResourceException($"El email ya está en uso: {requestDto.Email}");
        }

        var person = new Person
        {
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            Email = requestDto.Email,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Persons.Add(person);
        await _context.SaveChangesAsync();

        var user = new User
        {
            Username = requestDto.Username,
            PasswordHash = _passwordHasher.HashPassword(requestDto.Password),
            PersonId = person.Id,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var role = await _context.Roles.FindAsync(requestDto.RoleId);
        if (role == null)
        {
            throw new ResourceNotFoundException($"Rol no encontrado con ID: {requestDto.RoleId}");
        }

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserListResponseDto>> ListUsersAsync(ListUserRequestDto? requestDto)
    {
        var users = await _context.Users
            .Include(u => u.Person)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .ToListAsync();

        return users.Select(user => new UserListResponseDto
        {
            UserId = (int)user.Id,
            Username = user.Username,
            Email = user.Person?.Email ?? string.Empty,
            FullName = user.Person != null ? $"{user.Person.FirstName} {user.Person.LastName}" : string.Empty,
            Roles = user.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => ur.Role!.Name)
                .ToList()
        }).ToList();
    }

    public async Task DeleteUserAsync(DeleteUserRequestDto requestDto)
    {
        var user = await _context.Users.FindAsync(requestDto.Id);
        if (user == null)
        {
            throw new ResourceNotFoundException($"No se encontró el usuario con el ID: {requestDto.Id}");
        }

        user.Activo = false;
        await _context.SaveChangesAsync();
    }
}

