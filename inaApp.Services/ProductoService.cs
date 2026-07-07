using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Producto;
using inaApp.Common.Response;
using inaApp.Entities;
using inaApp.Repository;
namespace inaApp.Services
{
    public class ProductoService : IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>
    {

        private readonly IGenericRepository<Producto> _productoRepo;
        private readonly IGenericRepository<Categoria> _categoriaRepo;
        private readonly IMapper _mapper;
        public ProductoService(IGenericRepository<Producto> productoRepo, IGenericRepository<Categoria> categoriaRepo, IMapper mapper)
        {
            _productoRepo = productoRepo;
            _mapper = mapper;
            _categoriaRepo = categoriaRepo;
        }

        public async Task<Response<ProductoResponseDTO>> ActualizarAsync(ProductoUpdateDTO entity)
        {

         
            var producto = await _productoRepo.obtenerPorIdAsync(entity.Id);
            if (producto == null)
                throw new NotFoundException($"El producto con el ID {entity.Id} no existe.");

            if (entity.Precio <= 0)
                throw new InvalidPriceException("El precio debe ser mayor a 0.");

            if (entity.Stock <= 0)
                throw new invalidStockException("El stock debe ser mayor a 0.");

        
            var productos = await _productoRepo.obtenerTodosAsync();
            if (productos.Any(p => p.Nombre.ToLower() == entity.Nombre.ToLower() && p.Id != entity.Id))
                throw new DuplicateNameException($"El nombre {entity.Nombre} ya existe.");

            var categoria = await _categoriaRepo.obtenerPorIdAsync(entity.CategoriaId);
            if (categoria == null)
                throw new NotFoundException($"La categoría con el ID {entity.CategoriaId} no existe.");

            if (!categoria.Estado)
                throw new InvalidOperationException($"La categoría con el ID {entity.CategoriaId} está inactiva.");

            _mapper.Map(entity, producto);
            producto.Estado = true;

            producto = await _productoRepo.ActualizarAsync(producto);

            producto = await _productoRepo.obtenerPorIdAsync(producto.Id);

            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(producto),
                Message = "Producto actualizado exitosamente",
                Success = true
            };
        }

        public async Task<Response<ProductoResponseDTO>> CrearAsync(ProductoCreateDTO entity)
        {
            
            if (entity.Precio <= 0)
            {
                throw new InvalidPriceException("El precio debe ser mayor a 0");
            }

            if (entity.Stock <= 0)
            {
                throw new invalidStockException("El stock debe ser mayor a 0");
            }

            var productos = await _productoRepo.obtenerTodosAsync();
            if (productos.Any(p => p.Nombre.ToLower() == entity.Nombre.ToLower()))
            {
                throw new DuplicateNameException($"El nombre {entity.Nombre} ya existe");
            }

            Producto producto = _mapper.Map<Producto>(entity);

            producto = await _productoRepo.CrearAsync(producto);

            producto = await _productoRepo.obtenerPorIdAsync(producto.Id);

            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(producto),
                Message = "Producto creado exitosamente",
                Success = true
            };


        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            
            return new Response<bool>
            {
                Data =  await _productoRepo.EliminarAsync(id),
                Message = "Producto eliminado exitosamente",
                Success = true
            };

        }

        public async Task<Response<ProductoResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var pro = await _productoRepo.obtenerPorIdAsync(id);

            if (pro is null)
            {
                throw new NotFoundException($"El producto con el id {id} no existe");
            }


            return new Response<ProductoResponseDTO>
            {
                Data = _mapper.Map<ProductoResponseDTO>(pro),
                Message = "Producto obtenido exitosamente",
                Success = true
            };
        }

        

        public async Task<Response<List<ProductoResponseDTO>>> ObtenerTodosAsync()
        {
            var listaProd = await _productoRepo.obtenerTodosAsync();
            if (!listaProd.Any())
                throw new NotFoundException("No hay productos registrados");


            return new Response<List<ProductoResponseDTO>>
            {
                Data = _mapper.Map<List<ProductoResponseDTO>>(listaProd),
                Message = "Producto obtenidos exitosamente",
                Success = true
            };
        }
       

    }
}
