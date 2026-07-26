using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs
{
   public class FacturaUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public string NumeroFactura { get; set; } = string.Empty;

        public decimal Descuento { get; set; }

        public List<FacturaDetalleUpdateDTO> Detalles { get; set; } = new();
    }
}
