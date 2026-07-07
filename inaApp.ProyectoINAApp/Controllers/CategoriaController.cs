using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Categoria;
using inaApp.Entities;
using inaApp.ProyectoINAApp.Models.Categoria;
using inaApp.ProyectoINAApp.Models.Producto;
using inaApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace inaApp.ProyectoINAApp.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(IGenericService<
            CategoriaResponseDTO, 
            CategoriaCreateDTO, 
            CategoriaUpdateDTO> categoriaService, 
            IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }




        // GET: CategoriaController
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                var categoryList = await _categoriaService.ObtenerTodosAsync();

                var categoryVM = _mapper.Map<List<CategoriaIndexViewModel>>(categoryList.Data);

                return View(categoryVM);
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["SuccessMessage"] = ex.Message;
                return View();

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View();
            }
        }



        // GET: CategoriaController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var category = await _categoriaService.ObtenerPorIdAsync(id);

                var categoryVM = _mapper.Map<CategoriaIndexViewModel>(category.Data);

                return View(categoryVM);
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
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View();
            }
        }



        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CategoriaCreateViewModel categoriaCreateVM) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(categoriaCreateVM);
                }

                var categoryCreateDTO = _mapper.Map<CategoriaCreateDTO>(categoriaCreateVM);

                var newCategory = await _categoriaService.CrearAsync(categoryCreateDTO);

                if (!newCategory.Success)
                {
                    ModelState.AddModelError(string.Empty, newCategory.Message);
                    return View(categoriaCreateVM);
                }

                TempData["SuccessMessage"] = "Categoria creada exitosamente.";

                return RedirectToAction(nameof(Index));

            }
            catch (inaApp.Common.Exceptions.DuplicateNameException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(categoriaCreateVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View(categoriaCreateVM);
            }
        }



        // GET: CategoriaController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var category = await _categoriaService.ObtenerPorIdAsync(id);

                if (!category.Success)
                {
                    TempData["ErrorMessage"] = category.Message;
                    return RedirectToAction(nameof(Index)); 
                }

               var categoryEditVM = _mapper.Map<CategoriaEditViewModel>(category.Data);

                return View(categoryEditVM);
            }
            catch 
            { 
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View();
            }
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
      [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CategoriaEditViewModel categoriaEditVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(categoriaEditVM);
                }

                var categoryUpdateDTO = _mapper.Map<CategoriaUpdateDTO>(categoriaEditVM);

                var updatedCategory = await _categoriaService.ActualizarAsync(categoryUpdateDTO);

             if (!updatedCategory.Success)
                {
                    ModelState.AddModelError(string.Empty, updatedCategory.Message);
                    return View(categoriaEditVM);
                }

                TempData["SuccessMessage"] = "Categoria actualizada exitosamente.";

                return RedirectToAction(nameof(Index));

            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return View(categoriaEditVM);
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return View(categoriaEditVM);
            }
            catch (inaApp.Common.Exceptions.DuplicateNameException ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return View(categoriaEditVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View(categoriaEditVM);
            }
        }

        
        
        // GET: CategoriaController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var category = await _categoriaService.ObtenerPorIdAsync(id);

                if (!category.Success)
                {
                    TempData["ErrorMessage"] = category.Message;
                    return RedirectToAction(nameof(Index));
                }

             var categoryDeleteVM = _mapper.Map<CategoriaIndexViewModel>(category.Data);

                return View(categoryDeleteVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View();
            }
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        { 
            try
            {
                var categoryDeleted = await _categoriaService.EliminarAsync(id);

              if (!categoryDeleted.Success)
                {
                    TempData["ErrorMessage"] = categoryDeleted.Message;
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Categoria eliminada exitosamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            catch (inaApp.Common.Exceptions.NotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            catch
            {
                TempData["ErrorMessage"] = "Error en el servidor. Contacte con el administrador.";
                return View();
            }
        }


    }
}
