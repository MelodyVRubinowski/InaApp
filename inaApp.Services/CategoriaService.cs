using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Common.Response;
using inaApp.DTOs.Categoria;
using inaApp.Entities;
using inaApp.Repository;

namespace inaApp.Services
{
    public class CategoriaService : IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO>
    {
        private readonly IGenericRepository<Categoria> _categoriaRepo;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericRepository<Categoria> categoriaRepo, IMapper mapper)
        {
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
        }

        public async Task<Response<CategoriaResponseDTO>> CrearAsync(CategoriaCreateDTO entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Nombre))
                throw new RequiredFieldException("El nombre es obligatorio.");

            var categorias = await _categoriaRepo.obtenerTodosAsync();
            if (categorias.Any(c => c.Nombre.ToLower() == entity.Nombre.ToLower()))
                throw new DuplicateNameException($"La categoría '{entity.Nombre}' ya existe.");

            var categoria = _mapper.Map<Categoria>(entity);
            categoria = await _categoriaRepo.CrearAsync(categoria);

            return new Response<CategoriaResponseDTO>
            {
                Data = _mapper.Map<CategoriaResponseDTO>(categoria),
                Message = "Categoría creada exitosamente.",
                Success = true
            };
        }

        public async Task<Response<CategoriaResponseDTO>> ActualizarAsync(CategoriaUpdateDTO entity)
        {
            var existe = await _categoriaRepo.obtenerPorIdAsync(entity.Id);
            if (existe == null)
                throw new NotFoundException($"La categoría con el ID {entity.Id} no existe.");

            var categorias = await _categoriaRepo.obtenerTodosAsync();
            if (categorias.Any(c => c.Nombre.ToLower() == entity.Nombre.ToLower() && c.Id != entity.Id))
                throw new DuplicateNameException($"La categoría '{entity.Nombre}' ya existe.");

            _mapper.Map(entity, existe);
            existe = await _categoriaRepo.ActualizarAsync(existe);

            return new Response<CategoriaResponseDTO>
            {
                Data = _mapper.Map<CategoriaResponseDTO>(existe),
                Message = "Categoría actualizada exitosamente.",
                Success = true
            };
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            var categoria = await _categoriaRepo.obtenerPorIdAsync(id);
            if (categoria == null)
            {
                var todas = await _categoriaRepo.obtenerTodosAsync();
                if (todas.Any(c => c.Id == id))
                    throw new InvalidOperationException($"La categoría con el ID {id} está inactiva.");
                throw new NotFoundException($"La categoría con el ID {id} no existe.");
            }

            var eliminado = await _categoriaRepo.EliminarAsync(id);

            return new Response<bool>   
            {
                Data = eliminado,
                Message = eliminado ? "Categoria eliminada exitosamente." : "Error al eliminar la categor�a.",
                Success = eliminado
            };
        }

        public async Task<Response<CategoriaResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var categoria = await _categoriaRepo.obtenerPorIdAsync(id);

            if (categoria == null)
            {
                var todas = await _categoriaRepo.obtenerTodosAsync();
                if (todas.Any(c => c.Id == id))
                    throw new InvalidOperationException($"La categoría con el ID {id} está inactiva.");
                throw new NotFoundException($"La categoría con el ID {id} no existe.");
            }

            return new Response<CategoriaResponseDTO>
            {
                Data = _mapper.Map<CategoriaResponseDTO>(categoria),
                Message = "Categoría obtenida exitosamente.",
                Success = true
            };
        }

        public async Task<Response<List<CategoriaResponseDTO>>> ObtenerTodosAsync()
        {
            
            var categorias = await _categoriaRepo.obtenerTodosAsync();

            categorias = categorias.Where(c => c.Estado).ToList();

            if (!categorias.Any())
                throw new NotFoundException("No hay categorías registradas.");

            return new Response<List<CategoriaResponseDTO>>
            {
                Data = _mapper.Map<List<CategoriaResponseDTO>>(categorias),
                Message = "Categorías obtenidas exitosamente.",
                Success = true
            };
        }
    }
}