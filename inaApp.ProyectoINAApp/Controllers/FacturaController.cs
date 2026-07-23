using inaApp.Data;
using inaApp.DTOs;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
using inaApp.DTOs.ViewModels;
using inaApp.Entities;
using inaApp.ProyectoINAApp.Models.Factura;
using inaApp.Services;
using inaApp.Web.ViewModels.Factura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InaApp.Web.Controllers // Cambia esto por el namespace de tu proyecto web (ej: webApiINA.Controllers)
{
    public class FacturaController : Controller
    {
        private readonly FacturaService _facturaService;
        private readonly ApplicationDbContext _context;

        public FacturaController(FacturaService facturaService, ApplicationDbContext context)
        {
            _facturaService = facturaService;
            _context = context;
        }

        // 1. INDEX: Listar facturas (Maestro)
        public async Task<IActionResult> Index()
        {
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .OrderByDescending(f => f.Fecha)
                .ToListAsync();

            var modelo = facturas.Select(f => new FacturaIndexViewModel
            {
                Id = f.Id,
                NumeroFactura = f.NumeroFactura,
                Fecha = f.Fecha,
                ClienteIdentificacion = f.Cliente.NumeroIdentificacion,
                ClienteNombre = f.Cliente.Nombre, // Asegúrate que tu entidad Cliente tenga 'Nombre'
                Subtotal = f.Subtotal,
                Impuesto = f.Impuesto,
                Descuento = f.Descuento,
                Total = f.Total,
                Estado = f.Estado
            }).ToList();

            return View(modelo);
        }

        // 2. CREATE (GET): Mostrar formulario vacío
        public async Task<IActionResult> Create()
        {
            var model = new FacturaCreateViewModel
            {
                // Cargar listas para los selects (Maestro y Detalle)
                ListaClientes = await _context.Cliente
                    .Select(c => new ClienteResponseDTO
                    {
                        Id = c.Id,
                        Nombre = c.Nombre ?? c.NumeroIdentificacion,
                        NumeroIdentificacion = c.NumeroIdentificacion
                    }).ToListAsync(),

                ListaProductos = await _context.Producto
                    .Select(p => new ProductoResponseDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        Stock = p.Stock
                    }).ToListAsync()
            };

            // Inicializar listas vacías para la tabla dinámica
        //    model.DetallesTemporales = new List<DetalleLineaViewModel>();

            return View(model);
        }

        // 3. CREATE (POST): Recibir datos (Maestro + Detalle) y guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacturaCreateViewModel model)
        {
            // Validar que el modelo tenga datos básicos
            if (!ModelState.IsValid)
            {
                // Recargar listas si hay error de validación para no perder el select
                model.ListaClientes = await _context.Cliente.Select(c => new ClienteResponseDTO { Id = c.Id, Nombre = c.Nombre ?? c.NumeroIdentificacion, NumeroIdentificacion = c.NumeroIdentificacion }).ToListAsync();
                model.ListaProductos = await _context.Producto.Select(p => new ProductoResponseDTO { Id = p.Id, Nombre = p.Nombre, Precio = p.Precio, Stock = p.Stock }).ToListAsync();
                return View(model);
            }

            // Validación de negocio: ¿Hay al menos un detalle?
            if (model.DetallesTemporales == null || !model.DetallesTemporales.Any())
            {
                TempData["Error"] = "Debe agregar al menos un producto a la factura.";
                model.ListaClientes = await _context.Cliente.Select(c => new ClienteResponseDTO { Id = c.Id, Nombre = c.Nombre ?? c.NumeroIdentificacion, NumeroIdentificacion = c.NumeroIdentificacion }).ToListAsync();
                model.ListaProductos = await _context.Producto.Select(p => new ProductoResponseDTO { Id = p.Id, Nombre = p.Nombre, Precio = p.Precio, Stock = p.Stock }).ToListAsync();
                return View(model);
            }

            try
            {
                // Mapear el ViewModel (con la lista temporal) al DTO que espera el Service
                var dto = new FacturaCreateDTO
                {
                    ClienteId = model.ClienteId,
                    NumeroFactura = model.NumeroFactura,
                    Descuento = model.Descuento,
                    // Convertimos la lista de ViewModels a la lista de DTOs
                    Detalles = model.DetallesTemporales.Select(d => new FacturaDetalleCreateDTO
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad
                    }).ToList()
                };

                // Ejecutar el servicio (Aquí ocurre la magia: Transacción, Cálculos, Stock)
                await _facturaService.CrearFacturaAsync(dto);

                TempData["Exito"] = $"Factura {dto.NumeroFactura} creada con éxito.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Capturar errores del Service (stock insuficiente, cliente no existe, etc.)
                TempData["Error"] = ex.Message;

                // Mantener los datos del formulario para que el usuario no los pierda
                model.ListaClientes = await _context.Cliente.Select(c => new ClienteResponseDTO { Id = c.Id, Nombre = c.Nombre ?? c.NumeroIdentificacion, NumeroIdentificacion = c.NumeroIdentificacion }).ToListAsync();
                model.ListaProductos = await _context.Producto.Select(p => new ProductoResponseDTO { Id = p.Id, Nombre = p.Nombre, Precio = p.Precio, Stock = p.Stock }).ToListAsync();

                // Mantener los detalles que ya agregó el usuario (si es posible, o limpiar si el error fue grave)
                // Aquí mantenemos la lista temporal para que el usuario corrija
                return View(model);
            }
        }

        // 4. DETAILS: Ver factura completa (Maestro + Detalle)
        public async Task<IActionResult> Details(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Detalles).ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null) return NotFound();

            var model = new FacturaDetalleViewModel
            {
                NumeroFactura = factura.NumeroFactura,
                Fecha = factura.Fecha,
                ClienteNombre = factura.Cliente.Nombre ?? factura.Cliente.NumeroIdentificacion,
                ClienteIdentificacion = factura.Cliente.NumeroIdentificacion,
                Estado = factura.Estado,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Descuento = factura.Descuento,
                Total = factura.Total,
                Detalles = factura.Detalles.Select(d => new FacturaDetailsViewModel
                {
                    ProductoNombre = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal,
                    Impuesto = d.Impuesto,
                    TotalLinea = d.TotalLinea
                }).ToList()
            };

            return View(model);
        }

        // 5. ANULAR: Cambiar estado (Maestro)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null) return NotFound();

            if (factura.Estado == "Anulada")
            {
                TempData["Error"] = "La factura ya estaba anulada.";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Lógica simple: Solo cambia el estado
            // Nota: En un sistema real, aquí también deberías revertir el stock si es necesario
            factura.Estado = "Anulada";

            await _context.SaveChangesAsync();

            TempData["Exito"] = "Factura anulada correctamente.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}