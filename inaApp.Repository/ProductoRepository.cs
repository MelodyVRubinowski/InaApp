using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class ProductoRepository : IGenericRepository<Producto>
    {

        //inyeccion para usar el db context osea la bd y sus datos
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
                //borrado logico
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
                return await _context.Producto.Include(p => p.Categoria).Where(x => x.Id == id && x.Estado == true).SingleOrDefaultAsync();//aqui meti lo de include para que salgan las cosas de categoria

                //if (entity is null)            
                //    throw new Exception("No se encontro la entidad");

                //return entity;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<List<Producto>> obtenerTodosAsync()
        {
            try
            {
                return await _context.Producto.Include(p => p.Categoria).AsNoTracking().Where(x=> x.Estado==true).ToListAsync();//aqui meti lo de include para que salgan las cosas de categoria
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
