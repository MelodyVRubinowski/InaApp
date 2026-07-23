namespace inaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaDetalleViewModel
    {
        public string NumeroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteIdentificacion { get; set; }
        public string Estado { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public List<FacturaDetailsViewModel> Detalles { get; set; } = new();
    }
}