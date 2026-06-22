using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class CategoriaRepository : IGenericRepository<Categoria>
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Categoria> CrearAsync(Categoria entity)
        {
            _context.Categoria.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Categoria> ActualizarAsync(Categoria entity)
        {
            _context.Categoria.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var categoria = await obtenerPorIdAsync(id);
            if (categoria == null) return false;

            categoria.Estado = false; 
            _context.Categoria.Update(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Categoria> obtenerPorIdAsync(int id)
        {
            return await _context.Categoria
                .Include(c => c.Productos)
                .Where(c => c.Id == id && c.Estado) 
                .SingleOrDefaultAsync();
        }

        public async Task<List<Categoria>> obtenerTodosAsync()
        {
            return await _context.Set<Categoria>()
                             .Include(c => c.Productos)
                             .ToListAsync();
        }
    }
}