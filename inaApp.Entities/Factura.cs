using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace inaApp.Entities
{
        [Table("tbFactura")]
        public class Factura
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }

    
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;


        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }

        public string Estado { get; set; } = "Activa"; 
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();

    }
}

