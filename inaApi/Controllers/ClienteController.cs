using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Entities;
using Microsoft.AspNetCore.Mvc;
using inaApp.DTOs.Cliente;
using inaApp.Common.Response;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : Controller
    {
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;

        public ClienteController(IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService)
        {       
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var response = await _clienteService.ObtenerTodosAsync();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error de servidor, contacte con el servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ByIdAsync(int id)
        {
            try
            {
                var response = await _clienteService.ObtenerPorIdAsync(id);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Error de servidor, contacte con el servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ClienteCreateDTO clienteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _clienteService.CrearAsync(clienteDTO);
                return Created("cliente creado", response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Error al eliminar, id invalido");

                var response = await _clienteService.EliminarAsync(id);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error de servidor, contacte con el servidor");
            }
        }

        [HttpPut]
        public async Task<ActionResult> EditAsync([FromBody] ClienteUpdateDTO clienteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _clienteService.ActualizarAsync(clienteDTO);
                return Created("cliente editado", response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}