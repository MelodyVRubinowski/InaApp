using inaApp.DTOs.Factura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inaApp.DTOs
{
    public class FacturaCreateDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }


        [Required(ErrorMessage = "El número de factura es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número de factura no puede exceder 20 caracteres.")]
        public string NumeroFactura { get; set; } = string.Empty;

        // CRÍTICO: El enunciado exige que la factura pertenezca a un cliente.
        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La fecha de la factura es obligatoria.")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Range(0, 999999999.99, ErrorMessage = "El descuento no puede ser negativo.")]
        public decimal Descuento { get; set; }

        // CRÍTICO: El enunciado exige "al menos un producto".
        [Required(ErrorMessage = "Debe agregar al menos un producto a la factura.")]
        [MinLength(1, ErrorMessage = "La factura debe contener al menos un detalle.")]
        public List<FacturaDetalleCreateDTO> Detalles { get; set; } = new();
    }
}