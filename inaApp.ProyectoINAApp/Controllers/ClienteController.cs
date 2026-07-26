using AutoMapper;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using InaApp.ProyectoInaApp.Models.Cliente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InaApp.ProyectoInaApp.Controllers
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
        public async Task<ActionResult> Index()
        {
            try
            {
                var listaClientes = await _clienteService.ObtenerTodosAsync();

                var ListViewMoel = _mapper.Map<List<ClienteIndexViewModel>>(listaClientes.Data);

                return View(ListViewMoel);
            }
            catch (NotFoundDbException ex)
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
            catch (NotNumberPositiveException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)
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
        public ActionResult CrearAsync()
        {
            ViewBag.TiposIdentificacion = new List<SelectListItem>
    {
         new SelectListItem { Value = "CedulaFisica", Text = "Cédula de Identidad" },
    new SelectListItem { Value = "CedulaJuridica", Text = "Cédula Jurídica" },
    new SelectListItem { Value = "DIMEX", Text = "DIMEX (Cédula de Residencia)" },
    new SelectListItem { Value = "NITE", Text = "NITE" },
    new SelectListItem { Value = "Pasaporte", Text = "Pasaporte" }
    };

            return View(new ClienteCreateViewModel());
        }
        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearAsync(ClienteCreateViewModel clienteCreateVM)
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
            catch (EntityExistDbException ex)
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
            catch (NotNumberPositiveException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
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

        // POST: ClienteController/Edit/5
        //es un post xq los forms en html solo acepta 2 metodos post y get, y el edit es un form q envia datos al servidor para actualizar un cliente. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ClienteEditViewModel clienteEditVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(clienteEditVM);
                }

                //paso de ViewModel a DTO para enviarlo al servicio
                var clienteUpdateDTO = _mapper.Map<ClienteUpdateDTO>(clienteEditVM);

                var updatedCliente = await _clienteService.ActualizarAsync(clienteUpdateDTO);

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y
                //devuelvo la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!updatedCliente.Success)
                {
                    ModelState.AddModelError(string.Empty, updatedCliente.Message);
                    return View(clienteEditVM);
                }

                TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (NotNumberPositiveException ex)//exeption personalizada q se lanza desde el servicio si el id es negativo.
            {
                TempData["ErrorMessage"] = ex.Message;

                return View(clienteEditVM);
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
            {
                TempData["ErrorMessage"] = ex.Message;

                return View(clienteEditVM);
            }
            catch (EntityExistDbException ex)//exeption personalizada q se lanza desde el servicio si el cliente ya existe en la base de datos.
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

                //si el servicio devuelve un error, agrego un mensaje de error al ModelState y devuelvo
                //la vista con los datos ingresados para que el usuario pueda corregirlos
                if (!cliente.Success)
                {
                    TempData["ErrorMessage"] = cliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                //paso de DTO a ViewModel para enviarlo a la vista
                var clienteDeleteVM = _mapper.Map<ClienteIndexViewModel>(cliente.Data);

                return View(clienteDeleteVM);
            }
            catch (NotNumberPositiveException ex)//exeption personalizada q se lanza desde el servicio si el id es negativo.
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
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
            catch (NotNumberPositiveException ex)//exeption personalizada q se lanza desde el servicio si el id es negativo.
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundDbException ex)//exeption personalizada q se lanza desde el servicio
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
