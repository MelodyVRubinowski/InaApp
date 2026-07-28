using AutoMapper;
using inaApp.DTOs;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Factura;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using inaApp.ProyectoINAApp.Models.Categoria;
using inaApp.ProyectoINAApp.Models.Factura;
using inaApp.ProyectoINAApp.Models.Producto;
using inaApp.Web.ViewModels.Factura;

namespace inaApp.ProyectoINAApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            // --- MAPEOS DE PRODUCTO (EXISTENTES) ---
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();
            CreateMap<ProductoResponseDTO, ProductoEditViewModel>();
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>();
            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>();

            // --- MAPEOS DE CATEGORIA (EXISTENTES) ---
            CreateMap<CategoriaResponseDTO, CategoriaIndexViewModel>();
            CreateMap<CategoriaResponseDTO, CategoriaEditViewModel>();
            CreateMap<CategoriaIndexViewModel, CategoriaResponseDTO>();
            CreateMap<CategoriaCreateViewModel, CategoriaCreateDTO>();
            CreateMap<CategoriaEditViewModel, CategoriaUpdateDTO>();


            // 1. Flujo de Creación (De la Vista -> DTO -> Entidad)
            CreateMap<FacturaCreateViewModel, FacturaCreateDTO>();
            CreateMap<FacturaCreateDTO, Factura>();
            CreateMap<FacturaDetalleCreateDTO, FacturaDetalle>();

            // 2. Flujo de Consulta y Listados (De la Entidad -> DTO -> Vista)
            CreateMap<Factura, FacturaResponseDTO>();
            CreateMap<Factura, FacturaListDTO>();
            CreateMap<FacturaDetalle, FacturaDetalleResponseDTO>();
            CreateMap<FacturaResponseDTO, FacturaDetailsViewModel>();
        }
    }
}