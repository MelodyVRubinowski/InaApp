using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static inaApp.Common.Enums.Enumeradores;

namespace inaApp.DTOs
{
    public class FacturaCreateDTO
    {
        public int ClienteId { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public decimal Descuento { get; set; }
        public List<FacturaDetalleCreateDTO> Detalles { get; set; } = new();
    }

    public class FacturaDetalleCreateDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}