using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.DTOs.Categoria;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using inaApp.ProyectoINAApp.Mapping;
using inaApp.Repository;
using inaApp.Services;
using inaApp.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace inaApp.ProyectoINAApp.Extensions
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddAplicationServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
          services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddAutoMapper(fg => { }, typeof(MappingProfile), typeof(WebMappingProfile));


           services.AddScoped<IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>, ProductoService>();
            services.AddScoped<IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>, ClienteService>();
            services.AddScoped<IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO>, CategoriaService>();


            services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();
            services.AddScoped<IGenericRepository<Categoria>, CategoriaRepository>();


            return services;
        }
    }
}
