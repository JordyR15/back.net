using Microsoft.EntityFrameworkCore;
using back.Entities;

namespace back.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<SalaCine> SalaCines { get; set; }
    public DbSet<PeliculaSalacine> PeliculaSalacines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar clave compuesta para PeliculaSalacine
        modelBuilder.Entity<PeliculaSalacine>()
            .HasKey(ps => new { ps.PeliculaId, ps.SalaCineId });

        // Configurar clave compuesta para UserRole
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        // Configurar relaciones
        modelBuilder.Entity<User>()
            .HasOne(u => u.Person)
            .WithMany()
            .HasForeignKey(u => u.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany()
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PeliculaSalacine>()
            .HasOne(ps => ps.Pelicula)
            .WithMany(p => p.PeliculaSalacines)
            .HasForeignKey(ps => ps.PeliculaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PeliculaSalacine>()
            .HasOne(ps => ps.SalaCine)
            .WithMany(s => s.PeliculaSalacines)
            .HasForeignKey(ps => ps.SalaCineId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices únicos
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Configurar soft delete para Users (filtrar activos)
        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.Activo);

        // Configurar soft delete para Peliculas
        modelBuilder.Entity<Pelicula>()
            .HasQueryFilter(p => p.Activo);

        // Configurar soft delete para SalaCines
        modelBuilder.Entity<SalaCine>()
            .HasQueryFilter(s => s.Activo);

        // Configurar soft delete para Roles
        modelBuilder.Entity<Role>()
            .HasQueryFilter(r => r.Activo);
    }
}

