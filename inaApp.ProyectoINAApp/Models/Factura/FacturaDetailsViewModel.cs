namespace inaApp.ProyectoINAApp.Models.Factura
{
    public class FacturaDetailsViewModel
    {
        public string ProductoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalLinea { get; set; }
    }
}