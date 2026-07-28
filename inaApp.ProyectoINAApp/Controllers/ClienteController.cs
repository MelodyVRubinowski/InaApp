using AutoMapper;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using InaApp.ProyectoINAApp.Models.Cliente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }




        // GET: ClienteController
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var listaClientes = await _clienteService.ObtenerTodosAsync();

                var ListViewMoel = _mapper.Map<List<ClienteIndexViewModel>>(listaClientes.Data);

                return View(ListViewMoel);
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";

                return View(); 
            }
        }

        // GET: ClienteController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                var clienteViewModel = _mapper.Map<ClienteIndexViewModel>(cliente.Data);

                return View(clienteViewModel);
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return View();
            }
        }

        // GET: ClienteController/Create
        [HttpGet]
        public ActionResult CreateAsync()
        {
            ViewBag.TiposIdentificacion = new List<SelectListItem>
    {
         new SelectListItem { Value = "CedulaFisica", Text = "Cédula de Identidad" },
    new SelectListItem { Value = "CedulaJuridica", Text = "Cédula Jurídica" },
    new SelectListItem { Value = "DIMEX", Text = "DIMEX (Cédula de Residencia)" },
    new SelectListItem { Value = "NITE", Text = "NITE" },
    new SelectListItem { Value = "Pasaporte", Text = "Pasaporte" }
    };

            return View();
        }
        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ClienteCreateViewModel clienteCreateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(clienteCreateVM);
                }

                var clienteCreateDTO = _mapper.Map<ClienteCreateDTO>(clienteCreateVM);

                var newCliente = await _clienteService.CrearAsync(clienteCreateDTO);

                if (!newCliente.Success)
                {
                    ModelState.AddModelError(string.Empty, newCliente.Message);
                    return View(clienteCreateVM);
                }

                TempData["SuccessMessage"] = "Cliente creado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch  (inaApp.Common.Exceptions.NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(clienteCreateVM);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";

                return View(clienteCreateVM);
            }
        }

        // GET: ClienteController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                if (!cliente.Success)
                {
                    TempData["ErrorMessage"] = cliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                var clienteEditVM = _mapper.Map<ClienteEditViewModel>(cliente.Data);

                return View(clienteEditVM);
          
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";
                return View();
            }
        }

        // POST: ClienteController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ClienteEditViewModel clienteEditVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TiposIdentificacion = new List<SelectListItem>
{
    new SelectListItem { Value = "CedulaFisica", Text = "Cédula de Identidad" },
    new SelectListItem { Value = "CedulaJuridica", Text = "Cédula Jurídica" },
    new SelectListItem { Value = "DIMEX", Text = "DIMEX (Cédula de Residencia)" },
    new SelectListItem { Value = "NITE", Text = "NITE" },
    new SelectListItem { Value = "Pasaporte", Text = "Pasaporte" }
};

                    return View(clienteEditVM);
                }

                var clienteUpdateDTO = _mapper.Map<ClienteUpdateDTO>(clienteEditVM);

                var updatedCliente = await _clienteService.ActualizarAsync(clienteUpdateDTO);
                if (!updatedCliente.Success)
                {
                    ModelState.AddModelError(string.Empty, updatedCliente.Message);
                    return View(clienteEditVM);
                }

                TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";

                return RedirectToAction(nameof(Index));
           
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return View(clienteEditVM);
        }
            catch
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";

                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

               if (!cliente.Success)
                {
                    TempData["ErrorMessage"] = cliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                //paso de DTO a ViewModel para enviarlo a la vista
                var clienteDeleteVM = _mapper.Map<ClienteIndexViewModel>(cliente.Data);

                return View(clienteDeleteVM);
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";

                return View();
            }
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        {
            try
            {
                var deletedCliente = await _clienteService.EliminarAsync(id);

                //si el servicio devuelve un error, agrego un mensaje de error al TempData
                //y redirijo a la vista Index para que se muestre la lista de productos actualizada
                if (!deletedCliente.Success)
                {
                    TempData["ErrorMessage"] = deletedCliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Cliente eliminada exitosamente.";

                return RedirectToAction(nameof(Index));
           
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error interno del servidor. Contacte con el administrador.";

                return View();
            }
        }
    }
}
