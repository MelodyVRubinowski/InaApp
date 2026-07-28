using inaApp.Data;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class FacturaDetalleRepository : IFacturaDetalleRepository
{
    private readonly ApplicationDbContext _context;
    public FacturaDetalleRepository(ApplicationDbContext context) => _context = context;

    public async Task AddRangeAsync(IEnumerable<FacturaDetalle> detalles)
    {
        await _context.FacturaDetalle.AddRangeAsync(detalles);
    }

}
}