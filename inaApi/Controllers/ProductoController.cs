    using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Entites;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IGenericService<Producto> _productoService;

        public ProductoController(IGenericService<Producto> productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var productos = await _productoService.obtenerTodosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var result = await _productoService.ObtenerPorIdAsync(id);

                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return NotFound();
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Producto producto)
        {
            try
            {
                var result = await _productoService.CrearAsync(producto);
                return Created("Producto Creado", result);
            }
            catch (InvalidPriceException ex) {
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Erro al crar el producto");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Producto producto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Id incorrecto");
                }

                var result = await _productoService.ActualizarAsync(id, producto);

                if (result is null)
                {
                    return NotFound("Producto no encontrado");
                }

                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Producto no encontrado");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
        

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Error al eliminar, id Icorrecto");
                }

                var result = await _productoService.EliminarAsync(id);

                return result ? Ok("Producto Eliminado") : BadRequest("Erro al eliminar el producot");
            }
            catch (Exception ex)
            {

                return StatusCode(500, " Error 500");
            }

        }
    }
}