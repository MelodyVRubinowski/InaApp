using System.ComponentModel.DataAnnotations.Schema;

namespace inaApp.Entities
{
    [Table("tbFacturaDetalle")]
    public class FacturaDetalle
    {
        public int Id { get; set; }

        public int FacturaId { get; set; }
        public Factura Factura { get; set; } = null!;

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalLinea { get; set; }
     
    }
}