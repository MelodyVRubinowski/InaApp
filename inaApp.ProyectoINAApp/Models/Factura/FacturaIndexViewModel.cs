using System;
using inaApp.DTOs.Factura;
using inaApp.ProyectoINAApp.Models.Factura;
using inaApp.Services;

namespace inaApp.DTOs.ViewModels
{
    public class FacturaIndexViewModel
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        
        public string ClienteIdentificacion { get; set; } = string.Empty;
        public string ClienteNombre { get; set; } = string.Empty; 
        
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<FacturaListDTO> Facturas { get; internal set; }
    }
}