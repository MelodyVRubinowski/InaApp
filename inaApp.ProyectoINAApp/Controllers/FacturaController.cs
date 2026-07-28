using AutoMapper;
using inaApp.Common.Exceptions;

using inaApp.Common.Interfaces;
using inaApp.DTOs;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Factura;
using inaApp.DTOs.Producto;
using inaApp.DTOs.ViewModels;

using inaApp.Data;
using inaApp.ProyectoINAApp.Models.Factura;
using inaApp.Web.ViewModels.Factura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inaApp.Services;


namespace InaApp.ProyectoINAApp.Controllers
{
    public class FacturaController : Controller
    {
        private readonly FacturaService _facturaService;
        private readonly IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;
        private readonly IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FacturaController(FacturaService facturaService,
            IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService,
            IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoService,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _facturaService = facturaService;
            _clienteService = clienteService;
            _productoService = productoService;
            _context = context;
            _mapper = mapper;
        }

        // 1. INDEX: Listar facturas (Maestro)
        public async Task<IActionResult> Index()
        {
            // 1. Obtener y mapear la lista de facturas
            var facturas = await _context.Factura
                .Include(f => f.Cliente)
                .OrderByDescending(f => f.Fecha)
                .Select(f => new FacturaIndexViewModel 
                {
                
                    NumeroFactura = f.NumeroFactura,
                    Fecha = f.Fecha,
                    ClienteNombre = f.Cliente.Nombre ?? f.Cliente.NumeroIdentificacion,
                    ClienteIdentificacion = f.Cliente.NumeroIdentificacion,
                    Subtotal = f.Subtotal,
                    Impuesto = f.Impuesto,
                    Descuento = f.Descuento,
                    Total = f.Total,
                
                   
                })
                .ToListAsync();

            // 2. Devolver la LISTA directamente a la vista
            return View(facturas);
        }
        // 2. CREATE (GET): Mostrar formulario vacío
        public async Task<IActionResult> Create()
        {
            // 1. Obtener Clientes (Normalmente esto no falla, pero por seguridad podrías hacerlo igual)
            var respuestaClientes = await _clienteService.ObtenerTodosAsync();
            var listaClientes = respuestaClientes.Data ?? new List<ClienteResponseDTO>();

            // 2. Obtener Productos con manejo de error específico
            List<ProductoResponseDTO> listaProductos;
            try
            {
                var respuestaProductos = await _productoService.ObtenerTodosAsync();
                listaProductos = respuestaProductos.Data ?? new List<ProductoResponseDTO>();
            }
            catch (Exception ex) when (ex is inaApp.Common.Exceptions.NotFoundException || ex.Message.Contains("No hay productos registrados"))
            {
              listaProductos = new List<ProductoResponseDTO>();

                // Opcional: Guardar un mensaje temporal para mostrar en la vista
                TempData["MensajeAdvertencia"] = "No hay productos registrados. Agrega productos para poder crear facturas.";
            }

            // 3. Creamos el modelo para la vista
            var model = new FacturaCreateViewModel
            {
              Fecha = DateTime.Now,

                // Mapear listas
                ListaClientes = listaClientes.Select(c => new ClienteResponseDTO
                {
                    Id = c.Id,
                    Nombre = c.Nombre ?? c.NumeroIdentificacion,
                    NumeroIdentificacion = c.NumeroIdentificacion
                }).ToList(),

                ListaProductos = listaProductos.Select(p => new ProductoResponseDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Stock = p.Stock
                }).ToList(),
            };

            return View(model);
        }
        // 3. CREATE (POST): Recibir datos (Maestro + Detalle) y guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(FacturaCreateViewModel model)
        {
            var respuestaClientes = await _clienteService.ObtenerTodosAsync();
            var respuestaProductos = await _productoService.ObtenerTodosAsync();

            // Asignamos la lista interna (.Data) o una lista vacía si falla
            var listaClientes = respuestaClientes.Data ?? new List<ClienteResponseDTO>();
            var listaProductos = respuestaProductos.Data ?? new List<ProductoResponseDTO>();

            // 2. Ahora sí, usamos .Select() sobre las listas extraídas
            model.ListaClientes = listaClientes.Select(c => new ClienteResponseDTO
            {
                Id = c.Id,
                Nombre = c.Nombre ?? c.NumeroIdentificacion,
                NumeroIdentificacion = c.NumeroIdentificacion
            }).ToList();

            model.ListaProductos = listaProductos.Select(p => new ProductoResponseDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Stock = p.Stock
            }).ToList();

             if (model.DetallesTemporales == null || !model.DetallesTemporales.Any())
            {
                TempData["Error"] = "Debe agregar al menos un producto a la factura.";
                return View(model);
            }

        

            try
            {
                // Mapear el ViewModel al DTO
                var dto = new FacturaCreateDTO
                {
                                // si existe en el ViewModel
                    NumeroFactura = model.NumeroFactura,
                    ClienteId = model.ClienteId,
                    Fecha = model.Fecha,
                    Descuento = model.Descuento,

                    Detalles = model.DetallesTemporales.Select(d => new FacturaDetalleCreateDTO
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad
                    }).ToList()
                };
                // Ejecutar el servicio
                var resultado = await _facturaService.CrearAsync(dto);

                TempData["Exito"] = $"Factura {dto.NumeroFactura} creada con éxito.";
                return RedirectToAction(nameof(Details), new { Id = resultado.Id });
            }
            catch (Exception ex)
            {
                // Capturar errores del Service
                TempData["Error"] = ex.Message;
                return View(model);
            }
        }
        // 4. DETAILS: Ver factura completa (Maestro + Detalle)
        public async Task<IActionResult> Details(int id)
        {
            var factura = await _context.Factura
                .Include(f => f.Cliente)
                .Include(f => f.FacturaDetalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
                return NotFound();

            var model = new FacturaDetalleViewModel
            {
                NumeroFactura = factura.NumeroFactura,
                Fecha = factura.Fecha,
                ClienteNombre = factura.Cliente.Nombre ?? factura.Cliente.NumeroIdentificacion,
                ClienteId = factura.Cliente.NumeroIdentificacion,
                Estado = factura.Estado,
                Subtotal = factura.Subtotal,
                Impuesto = factura.Impuesto,
                Descuento = factura.Descuento,
                Total = factura.Total,
                Detalles = factura.FacturaDetalles.Select(d => new FacturaDetailsViewModel
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
            var factura = await _context.Factura.FindAsync(id);
            if (factura == null)
                return NotFound();

            if (factura.Estado == "Anulada")
            {
                TempData["Error"] = "La factura ya estaba anulada.";
                return RedirectToAction(nameof(Details), new { id });
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
       }   