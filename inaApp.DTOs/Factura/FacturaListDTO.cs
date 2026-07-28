using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.DTOs.Factura
{
    public class FacturaListDTO
    {
        [Key]
        [Required(ErrorMessage = "El ID es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de factura es obligatorio.")]
        [StringLength(50, ErrorMessage = "El número de factura no puede exceder los 50 caracteres.")]
        [RegularExpression(@"^[A-Z0-9-]+$", ErrorMessage = "El formato del número de factura es inválido.")]
        public string NumeroFactura { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del cliente no puede exceder los 100 caracteres.")]
        public string ClienteNombre { get; set; }

        [Required(ErrorMessage = "El subtotal es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El subtotal debe ser un valor positivo mayor a 0.")]
        public decimal Subtotal { get; set; }

        [Range(0, 999999999.99, ErrorMessage = "El impuesto no puede ser negativo.")]
        public decimal Impuesto { get; set; }

        [Range(0, 999999999.99, ErrorMessage = "El descuento no puede ser negativo ni mayor al total.")]
        public decimal Descuento { get; set; }

        [Required(ErrorMessage = "El total es obligatorio.")]
        [Range(0.01, 999999999.99, ErrorMessage = "El total debe ser un valor positivo mayor a 0.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres.")]
        public string Estado { get; set; }
    }
}