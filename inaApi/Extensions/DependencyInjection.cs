using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.DTOs.Producto;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Categoria;
using inaApp.Entities;
using inaApp.Repository;
using inaApp.Services;
using Microsoft.EntityFrameworkCore;
using inaApp.Services.Mapping;

namespace inaApp.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //base de datos- db context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            //Profile auto mapper 
            //services.AddAutoMapper(typeof(MappingProfile));
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

            //inyecciones de dependencia de servicios
            services.AddScoped<IGenericService<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>, ProductoService>();
            services.AddScoped<IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>, ClienteService>();
            services.AddScoped<IGenericService<CategoriaResponseDTO, CategoriaCreateDTO, CategoriaUpdateDTO>, CategoriaService>();
            //inyecciones de  dependencia de repositorios
            services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();
            services.AddScoped<IGenericRepository<Categoria>, CategoriaRepository>();

            return services;
        }
    }
}
