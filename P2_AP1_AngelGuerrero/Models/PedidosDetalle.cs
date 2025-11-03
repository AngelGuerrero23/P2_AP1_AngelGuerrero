using System.ComponentModel.DataAnnotations;

namespace P2_AP1_AngelGuerrero.Models;

public class PedidosDetalle
{
    [Key]
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ComponenteId { get; set; }
    [Range(0, int.MaxValue)]
    public int Cantidad { get; set; }
    [Range(0, int.MaxValue)]
    public double precio { get; set; }
}
