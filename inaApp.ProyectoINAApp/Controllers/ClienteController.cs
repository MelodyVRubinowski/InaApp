using AutoMapper;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using inaApp.Entities;
using inaApp.ProyectoINAApp.Models.Cliente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inaApp.ProyectoINAApp.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(
            IGenericService<ClienteResponseDTO,
            ClienteCreateDTO,
            ClienteUpdateDTO> clienteService,
            IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        // GET: Cliente
        public async Task<ActionResult> Index()
        {
            try
            {
                var clientes = await _clienteService.ObtenerTodosAsync();

                var lista = _mapper.Map<List<ClienteIndexViewModel>>(clientes.Data);

                return View(lista);
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
            catch
            {
                ViewBag.ErrorMessage = "Error interno del servidor.";
                return View();
            }
        }

        // GET: Cliente/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                var viewModel = _mapper.Map<ClienteIndexViewModel>(cliente.Data);

                return View(viewModel);
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
            catch
            {
                TempData["ErrorMessage"] = "Error interno del servidor.";
                return RedirectToAction(nameof(Index));
            }
        }
        // GET: Cliente/Create
        [HttpGet]
        public ActionResult Create()
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
        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClienteCreateViewModel clienteVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(clienteVM);

                var dto = _mapper.Map<ClienteCreateDTO>(clienteVM);

                var response = await _clienteService.CrearAsync(dto);

                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(clienteVM);
                }

                TempData["SuccessMessage"] = "Cliente creado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(clienteVM);
            }
        }

        // GET: Cliente/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                if (!cliente.Success)
                {
                    TempData["ErrorMessage"] = cliente.Message;
                    return RedirectToAction(nameof(Index));
                }

                var viewModel = _mapper.Map<ClienteEditViewModel>(cliente.Data);

                ViewBag.TiposIdentificacion = new List<SelectListItem>
        {
             new SelectListItem { Value = "CedulaFisica", Text = "Cédula de Identidad" },
    new SelectListItem { Value = "CedulaJuridica", Text = "Cédula Jurídica" },
    new SelectListItem { Value = "DIMEX", Text = "DIMEX (Cédula de Residencia)" },
    new SelectListItem { Value = "NITE", Text = "NITE" },
    new SelectListItem { Value = "Pasaporte", Text = "Pasaporte" }
        };

                return View(viewModel);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error interno del servidor.";
                return RedirectToAction(nameof(Index));
            }
        }
        // POST: Cliente/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClienteEditViewModel clienteVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(clienteVM);

                var dto = _mapper.Map<ClienteUpdateDTO>(clienteVM);

                var response = await _clienteService.ActualizarAsync(dto);

                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(clienteVM);
                }

                TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(clienteVM);
            }
        }

        // GET: Cliente/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _clienteService.ObtenerPorIdAsync(id);

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<ClienteIndexViewModel>(response.Data);

            return View(viewModel);
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _clienteService.EliminarAsync(id);

                if (!response.Success)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Cliente eliminado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}