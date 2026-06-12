using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class ProductoService : IGenericService<Producto>
    {

        //Implementar a inyección de dependecias

        private readonly IGenericRepository<Producto> _producRepository;

        public ProductoService(IGenericRepository<Producto> productRepo)
        {
            this._producRepository = productRepo;
        }

        public async Task<Producto> ActualizarAsync(int id,Producto entity)
        {
            //Validar precio
            if (entity.Precio <= 0) throw new InvalidPriceException("El precio debe ser una cantidad positiva");

            //Valida stock
            if (entity.Stock <= 0) throw new InvalidStockException("El stock debe ser una cantidad positiva");

            var resul= await _producRepository.ActualizarAsync(id,entity);

            if (resul is null)
            {
                throw new NotFoundException($"Usuario no encontrado");
            }

            return resul;
        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            try
            {
                //Validar precio
                if (entity.Precio<=0) throw new InvalidPriceException("El precio debe ser una cantidad positiva");

                //Valida stock
                if (entity.Stock <= 0) throw new InvalidStockException("El stock debe ser una cantidad positiva");

                //Validar nombre
                if (await _producRepository.validarNombreRepetido(entity.Nombre)) throw new DuplicateNameException($"El nombre {entity.Nombre} ya se encuentra agregado como producto");

                //Returnamos la respuesta del repositorio
                return await _producRepository.CrearAsync(entity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _producRepository.EliminarAsync(id);
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            try
            {
                var producto = await _producRepository.ObtenerPorIdAsync(id);

                if (producto is null)
                {
                    throw new NotFoundException($"EL producto con id {id} no existe");
                }

                return producto;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<List<Producto>> obtenerTodosAsync()
        {
            return await _producRepository.obtenerTodosAsync();
        }
    }
}
