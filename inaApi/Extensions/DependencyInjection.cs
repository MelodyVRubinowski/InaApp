using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entites;
using inaApp.Repository;
using inaApp.Services;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplicaServices(this IServiceCollection services,IConfiguration configuration)
        {
            //Inyecciones de baseDatos-dbContex
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //Inyecciones de dependencia de servicios
            services.AddScoped<IGenericService<Producto>, ProductoService>();
            services.AddScoped<IGenericService<Cliente>, ClienteService>();

            //Inyección de dependencia de repostorios
            services.AddScoped<IGenericRepository<Producto>, ProductoRespository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();
            services.AddScoped<IGenericRepository<Empleado>, EmpleadoRespository>();

            return services;
        }
    }
}
