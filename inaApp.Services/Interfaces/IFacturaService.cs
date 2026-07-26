using inaApp.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inaApp.Services.Interfaces
{
    public interface IFacturaService
    {
        Task<FacturaResponseDTO> CrearFacturaAsync(FacturaCreateDTO dto);
        Task<FacturaResponseDTO> AnularFacturaAsync(int id);
        Task<FacturaResponseDTO> ObtenerPorIdAsync(int id);
        Task<List<FacturaListDTO>> ObtenerTodasAsync(); // Nota: También cambiamos List<FacturaListDTO>
        (decimal Subtotal, decimal Impuesto, decimal Total) CalcularTotales(List<FacturaDetalleCreateDTO> detalles, decimal descuento = 0);
    }
}