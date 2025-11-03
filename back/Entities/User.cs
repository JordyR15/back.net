using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace back.Entities;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Required]
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("reset_token")]
    public string? ResetToken { get; set; }

    [Column("reset_token_expiry")]
    public DateTimeOffset? ResetTokenExpiry { get; set; }

    [Required]
    [Column("person_id")]
    public long PersonId { get; set; }

    [ForeignKey("PersonId")]
    public Person Person { get; set; } = null!;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

