using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;

namespace inaApp.Web.ViewModels.Factura
{
    public class FacturaCreateViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        [Display(Name = "Cédula del Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El número de factura es obligatorio.")]
        [Display(Name = "Número de Factura")]
        public string NumeroFactura { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo.")]
        public decimal Descuento { get; set; }

        public List<FacturaDetalleCreateViewModel> Detalles { get; set; } = new();

        // Lista para el dropdown de clientes
        public IEnumerable<SelectListItem> Clientes { get; set; } = new List<SelectListItem>();

        // Lista para el dropdown de productos
        public IEnumerable<SelectListItem> Productos { get; set; } = new List<SelectListItem>();
        public List<ClienteResponseDTO> ListaClientes { get; set; } = new();

        public List<ProductoResponseDTO> ListaProductos { get; set; } = new();

        public List<FacturaDetalleCreateViewModel> DetallesTemporales { get; set; } = new();
    }

    public class FacturaDetalleCreateViewModel
    {
        [Required]
        public int ProductoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
    }
}