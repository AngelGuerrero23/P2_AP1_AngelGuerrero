using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2_AP1_AngelGuerrero.Models;

public class Pedidos
{
    [Key]
    public int PedidoId { get; set; }

    [Required]
    public DateTime Fecha { get; set; }= DateTime.Now;
    [Required(ErrorMessage ="El nombre del cliente es obligatorio")]
    public string NombreCliente { get; set; }
    [Range(0, int.MaxValue)]
    public double Total { get; set; }

    [ForeignKey("PedidoId")]
    public virtual ICollection<PedidosDetalle> GetPedidosDetalles { get; set; } = new List<PedidosDetalle>();
}
