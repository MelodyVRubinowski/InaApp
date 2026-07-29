using System;

namespace inaApp.DTOs.Producto
{
    public class ProductoResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Descripcion { get; set; }
        public bool Estado { get; set; }
        public int CategoriaId { get; set; }
        public string? NombreCategoria { get; set; } // Útil para mostrar el nombre de la categoría en lugar del ID

        // --- NUEVOS CAMPOS PARA VISUALIZACIÓN ---

        public string ImpuestoAplicable { get; set; } = null!; // Ejemplo: "IVA"
        public decimal PorcentajeImpuesto { get; set; }       // Ejemplo: 13.00
        public decimal DescuentoMaximoPermitido { get; set; } // Ejemplo: 10.00
    }
}