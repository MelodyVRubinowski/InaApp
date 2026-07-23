using System;
using System.Collections.Generic;

namespace inaApp.Entities
{
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

   
        public List<FacturaDetalle> Detalles { get; set; } = new();
    }
}