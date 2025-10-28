using System.ComponentModel.DataAnnotations;

namespace P2_AP1_AngelGuerrero.Models;

public class Generico
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Fecha { get; set; }= DateTime.Now;

}
