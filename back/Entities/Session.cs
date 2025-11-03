using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("sessions")]
public class Session
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("user_id")]
    public long? UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required]
    [Column("id_session")]
    public string IdSession { get; set; } = string.Empty;

    [Column("session_active")]
    public bool? SessionActive { get; set; }

    [Column("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [Required]
    [Column("expires_at")]
    public DateTimeOffset ExpiresAt { get; set; }
}

