using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("pelicula_salacine")]
public class PeliculaSalacine
{
    [Column("id_pelicula")]
    public int PeliculaId { get; set; }

    [Column("id_sala_cine")]
    public int SalaCineId { get; set; }

    [ForeignKey("PeliculaId")]
    public Pelicula Pelicula { get; set; } = null!;

    [ForeignKey("SalaCineId")]
    public SalaCine SalaCine { get; set; } = null!;

    [Column("fecha_publicacion", TypeName = "date")]
    public DateTime? FechaPublicacion { get; set; }

    [Column("fecha_fin", TypeName = "date")]
    public DateTime? FechaFin { get; set; }

    [Column("id_pelicula_sala")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? IdPeliculaSala { get; set; }
}

