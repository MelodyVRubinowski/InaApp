using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class ProductoRespository : IGenericRepository<Producto>
    {

        private readonly ApplicationDbContext _context;

        public ProductoRespository(ApplicationDbContext context)
        {
            _context = context;   
        }
        public async Task<Producto> ActualizarAsync(int id,Producto entity)
        {
            try
            {
                var producto = await _context.Producto.Where(x => x.Id == id && x.Estado == true).FirstOrDefaultAsync();

                if (producto is null)
                {
                    return null;
                }

                await _context.Producto
                    .Where(x => x.Id == id && x.Estado == true)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Nombre, entity.Nombre)
                    .SetProperty(p => p.Precio, entity.Precio)
                    .SetProperty(p => p.Descripcion, entity.Descripcion)
                    );

                //Seteamos entidad
                entity.Id = id;
                entity.Estado = true;

                //Retornamos la entidad
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
                await _context.Producto.AddAsync(entity);
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
                var producto = await ObtenerPorIdAsync(id);
                if (producto==null)
                {
                    return false;
                }
                producto.Estado = false;
                _context.Producto.Update(producto);
                _context.SaveChanges();
                return true;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _context.Producto.Where(x => x.Id == id && x.Estado == true).SingleOrDefaultAsync();
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
                //Obtener lista de Productos, donde los estados este activos|
                return await _context.Producto.Where(x=> x.Estado==true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> validarNombreRepetido(string nombre) { 
            return await _context.Producto.AnyAsync(x => x.Nombre == nombre);
        }

        
    }
}
