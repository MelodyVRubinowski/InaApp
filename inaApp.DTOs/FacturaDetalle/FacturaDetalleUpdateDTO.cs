using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs
{
    public class FacturaDetalleUpdateDTO
    {

        public string ProductoNombre { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteIdentificacion { get; set; }
        public string Estado { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
public List<FacturaDetalleUpdateDTO> Detalles { get; set; } = new();
    }
}

