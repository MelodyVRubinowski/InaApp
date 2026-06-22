using System.Collections.Generic;
using inaApp.Entities;
using inaApp.DTOs.Producto;

namespace inaApp.DTOs.Categoria
{
    public class CategoriaResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; } 

        public ICollection<ProductoResponseDTO> Productos { get; set; } = new List<ProductoResponseDTO>();
    }
}