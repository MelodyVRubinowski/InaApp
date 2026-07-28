using AutoMapper;
using inaApp.DTOs;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Factura;
using inaApp.DTOs.Producto;
using inaApp.Entities;

namespace inaApp.Services.Mapping
{
    public class MappingProfile : Profile
    {

            public MappingProfile()
        {

            CreateMap<ProductoCreateDTO, Producto>();
            CreateMap<ClienteCreateDTO, Cliente>();
            CreateMap<CategoriaCreateDTO, Categoria>();

            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<ClienteUpdateDTO, Cliente>();
            CreateMap<CategoriaUpdateDTO, Categoria>();

            CreateMap<Producto, ProductoResponseDTO>().ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Nombre)); 
            CreateMap<Cliente, ClienteResponseDTO>();
            CreateMap<Categoria, CategoriaResponseDTO>().ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.Productos));

            CreateMap<Factura, FacturaCreateDTO>();
            CreateMap<FacturaCreateDTO, Factura>();
            CreateMap<FacturaDetalleCreateDTO, FacturaDetalle>();

            CreateMap<Factura, FacturaResponseDTO>();
            CreateMap<Factura, FacturaListDTO>();
            CreateMap<FacturaDetalle, FacturaDetalleResponseDTO>();
            CreateMap<FacturaResponseDTO, Factura>();
        }

    }


    }

