using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class ProductoRepository : IGenericRepository<Producto>
    {
private readonly ApplicationDbContext _context;
        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            try
            {
                _context.Producto.Update(entity);
                await _context.SaveChangesAsync();  
                return entity; 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            try
            {
                _context.Producto.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var producto= await obtenerPorIdAsync(id);
                if (producto == null) 
                {
                    return false;
                }

                producto.Estado = false;
                _context.Producto.Update(producto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Producto> obtenerPorIdAsync(int id)
        {
            try
            {
                return await _context.Producto.Include(p => p.Categoria).
                    Where(x => x.Id == id && x.Estado == true).
                    SingleOrDefaultAsync();

                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<Producto>> BuscarProductosAsync(string filtro)
        {
            // Requerimiento 4: Filtrar por código (Id) o nombre
            return await _context.Producto
                .Include(p => p.Categoria)
                .Where(x => x.Estado == true &&
                           (x.Nombre.Contains(filtro) || x.Id.ToString() == filtro))
                .ToListAsync();
        }

        public async Task<List<Producto>> obtenerTodosAsync()
        {
            try
            {
                return await _context.Producto.Include(p => p.Categoria).AsNoTracking().Where(x=> x.Estado==true).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
