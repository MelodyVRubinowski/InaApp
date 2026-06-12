using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        //Inyección de dependencias
        private readonly IGenericService<Cliente> _clienteService;

        public ClienteController(IGenericService<Cliente> clienteService)
        {
            this._clienteService = clienteService;
        }

        // GET: ClienteController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            //Manejamos errores en ejecución
            try
            {
                //Retornamos un Ok de lo que devuelva el service de obtener todo
                return Ok(await _clienteService.obtenerTodosAsync());
            }
            catch (Exception)
            {
                return BadRequest("Error al mostrar todos lo Clientes");
            }
        }

        // GET: ClienteController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                //Retornamos el cliente obtenido por el id
                return Ok(await _clienteService.ObtenerPorIdAsync(id));
            }
            //Capturamos errores de validaciones de negocio
            catch (BusinessValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Error al buscar el cliente");
            }
        }

        // POST: ClienteController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Cliente cliente)
        {
            try
            {
                //Creamos el cliente
                var result = await _clienteService.CrearAsync(cliente);

                //Retornamos el cliente creado
                return Created($"/api/clientes/{result.IdCliente}", result);
            }
            //Capturamos errores de validaciones de negocio
            catch (BusinessValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateClientIdentificationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ClientEmailAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Error al crear el cliente");
            }
        }

        // POST: ClienteController/Edit/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, Cliente Cliente)
        {
            try
            {
                //Actualizamos el cliente
                return Ok(await _clienteService.ActualizarAsync(id, Cliente));
            }
            //Capturamos errores de validaciones de negocio
            catch (BusinessValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateClientIdentificationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ClientEmailAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Error al Acutualizar el cliente");
            }
        }

        // DELETE: ClienteController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                //Eliminamos el cliente
                return Ok(await _clienteService.EliminarAsync(id));
            }
            //Capturamos errores de validaciones de negocio
            catch (BusinessValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound("Error al Eliminar el cliente");
            }
            catch (Exception)
            {
                return BadRequest("Error al Eliminar el cliente");
            }
        }
    }
}