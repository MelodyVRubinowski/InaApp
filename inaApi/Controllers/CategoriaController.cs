using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using inaApp.Common.Exceptions;
using inaApp.Common.Response;
using inaApp.DTOs.Categoria;
using inaApp.Services;
using Microsoft.AspNetCore.Mvc;
using inaApp.Common.interfaces;

namespace inaApi.Controllers
{
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController : Controller
    {
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;

        public CategoriaController(IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _categoriaService.ObtenerTodosAsync());
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (Exception) { return StatusCode(500, "Error de servidor, contacte con el administrador"); }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                return Ok(await _categoriaService.ObtenerPorIdAsync(id));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
            catch (Exception) { return StatusCode(500, "Error de servidor, contacte con el administrador"); }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await _categoriaService.CrearAsync(dto));
            }
            catch (RequiredFieldException ex) { return BadRequest(ex.Message); }
            catch (DuplicateNameException ex) { return BadRequest(ex.Message); }
            catch (Exception) { return StatusCode(500, "Error de servidor, contacte con el administrador"); }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] CategoriaUpdateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(await _categoriaService.ActualizarAsync(dto));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (DuplicateNameException ex) { return BadRequest(ex.Message); }
            catch (Exception) { return StatusCode(500, "Error de servidor, contacte con el administrador"); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _categoriaService.EliminarAsync(id));
            }
            catch (NotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(ex.Message); }
            catch (Exception) { return StatusCode(500, "Error de servidor, contacte con el administrador"); }
        }
    }
}