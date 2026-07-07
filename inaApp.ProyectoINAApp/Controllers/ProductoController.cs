using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using inaApp.ProyectoINAApp.Models.Producto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Threading.Tasks;


namespace inaApp.ProyectoINAApp.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public ProductoController(
            IGenericService<ProductoResponseDTO, 
            ProductoCreateDTO, 
            ProductoUpdateDTO> productoService,
            IGenericService<CategoriaResponseDTO, 
            CategoriaCreateDTO, 
            CategoriaUpdateDTO> categoriaService,
            IMapper mapper)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }




        public async Task<ActionResult> Index()
        {
            try
            {
                var listProducts = await _productoService.ObtenerTodosAsync();

                var ListViewModel = _mapper.Map<List<ProductoIndexViewModel>>(listProducts.Data);

          
                return View(ListViewModel);
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Error interno del servidor. Contacte con el administrador.";
                return View();

            }

        }

        // GET: ProductoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerPorIdAsync(id);

                var productoDetailsVM = _mapper.Map<ProductoIndexViewModel>(producto.Data);

                return View(productoDetailsVM);
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



        // GET: ProductoController/Create
        [HttpGet]
        public async Task<ActionResult> CreateAsync()
        {
            var categorias = await _categoriaService.ObtenerTodosAsync();

            var viewModel = new ProductoCreateViewModel
            {
                Categorias = new SelectList(categorias.Data, "Id", "Nombre")
            };

            return View(viewModel);
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> CreateAsync(ProductoCreateViewModel productoVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var categorias = await _categoriaService.ObtenerTodosAsync();
                    productoVM.Categorias = new SelectList(categorias.Data, "Id", "Nombre");

                    return View(productoVM);
                }

                var productoCreateDTO = _mapper.Map<ProductoCreateDTO>(productoVM);

                var response = await _productoService.CrearAsync(productoCreateDTO);


                if (!response.Success)
                {
                    var categorias = await _categoriaService.ObtenerTodosAsync();
                    productoVM.Categorias = new SelectList(categorias.Data, "Id", "Nombre");

                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(productoVM);
                }

                TempData["SuccessMessage"] = "Producto creado exitosamente.";

                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        

        // GET: ProductoController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var product = await _productoService.ObtenerPorIdAsync(id);

                if (!product.Success)
                {
                    TempData["ErrorMessage"] = product.Message;
                    return RedirectToAction(nameof(Index));
                }

              var productoEditVM = _mapper.Map<ProductoEditViewModel>(product.Data);

                var categorias = await _categoriaService.ObtenerTodosAsync();

                productoEditVM.Categorias = new SelectList(categorias.Data, "Id", "Nombre");

                return View(productoEditVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ProductoEditViewModel productoEditVM)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var categorias = await _categoriaService.ObtenerTodosAsync();

                    productoEditVM.Categorias = new SelectList(categorias.Data, "Id", "Nombre");

                    return View(productoEditVM);
                }

                var productoUpdateDTO = _mapper.Map<ProductoUpdateDTO>(productoEditVM);

                var response = await _productoService.ActualizarAsync(productoUpdateDTO);


                if (!response.Success)
                {
                    var categorias = await _categoriaService.ObtenerTodosAsync();

  
                    productoEditVM.Categorias = new SelectList(categorias.Data, "Id", "Nombre");

                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(productoEditVM);
                }

                TempData["SuccessMessage"] = "Producto modificado exitosamente.";

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        
        // GET: ProductoController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var response = await _productoService.ObtenerPorIdAsync(id);

            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction(nameof(Index));
            }

            var productoDeleteVM = _mapper.Map<ProductoIndexViewModel>(response.Data);

            return View(productoDeleteVM);
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        {
            try
            {
                var response = await _productoService.EliminarAsync(id);

                if (!response.Success)
                {
                    TempData["ErrorMessage"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
                
        }
               

    }
}
