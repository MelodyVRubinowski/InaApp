using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using inaApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoController : Controller
    {

        //inyeccion de dependencia
        private readonly IGenericService<ProductoResponseDTO,ProductoCreateDTO,ProductoUpdateDTO> _productoService;
        

        public ProductoController(IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoServ)
        {
            _productoService = productoServ;
            
        }

        // GET: ProductoController

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {

            try
            {
                var response = await _productoService.ObtenerTodosAsync();

                return Ok(response);
            }
            catch (NotFoundException ex) 
            { 
                return NotFound(ex.Message);
            }
            catch (Exception)
            {

                return StatusCode(500, "Error de servidor,contacte con el servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ByIdAsync(int id)
        {

            try
            {
                var producto = await _productoService.ObtenerPorIdAsync(id);

                //if (result == null)
                //{
                //    return NotFound("Producto no encontrado");
                //}

                return Ok(producto);
            }
            catch (NotFoundException ex)//aqui usammos la exepcion personalizada
            {
                return NotFound(ex.Message);
            }
            catch { 
                return StatusCode(500, "Error de servidor,contacte con el servidor");
            }
        }

        // GET: ProductoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //// GET: ProductoController/Create
        //public ActionResult Create()
        //{



        //    return View();
        //}

        // POST: ProductoController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductoCreateDTO productoDTO)
        {
            try
            {

              //  productoDTO.Estado = true;

                //Validar los datos de entrada
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _productoService.CrearAsync(productoDTO);

                return Created("producto creado", response);
            }
            catch (InvalidPriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (invalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500,
                    "Error de servidor, contacte con el administrador");
            }
        }

        //// GET: ProductoController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ProductoController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(IndexAsync));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: ProductoController/Delete/5

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Error al eliminar, id invalido");

                var response = await _productoService.EliminarAsync(id);

                return response.Data ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {

                return StatusCode (500,"Error de servidor,contacte con el servidor");
            }

        }

        [HttpPut]
        public async Task<ActionResult> EditAsync([FromBody] ProductoUpdateDTO producto)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                // producto.Estado = true;
                var response = await _productoService.ActualizarAsync(producto);

                return Ok(response);
            }
            catch (InvalidPriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (invalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500,
                    "Error de servidor, contacte con el administrador");
            }
        }


    }
}
