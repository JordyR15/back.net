using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("pelicula")]
public class Pelicula
{
    [Key]
    [Column("id_pelicula")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("duracion")]
    public int? Duracion { get; set; }

    [Required]
    [Column("activo")]
    public bool Activo { get; set; } = true;

    public ICollection<PeliculaSalacine> PeliculaSalacines { get; set; } = new List<PeliculaSalacine>();
}

