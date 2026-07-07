using inaApp.Common.Interfaces;
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

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
