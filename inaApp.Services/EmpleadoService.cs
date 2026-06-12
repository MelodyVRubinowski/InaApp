using inaApp.Common.Interfaces;
using inaApp.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class EmpleadoService : IGenericService<Empleado>
    {
        //Inyección de dependencias
        private readonly IGenericRepository<Empleado> repository;

        //Metodo constructor de dependecias
        public EmpleadoService(IGenericRepository<Empleado> repo)
        {
            this.repository = repo;
        }

        public Task<Empleado> ActualizarAsync(int id, Empleado entity)
        {
            throw new NotImplementedException();
        }

        public Task<Empleado> CrearAsync(Empleado entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Empleado> ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Empleado>> obtenerTodosAsync()
        {
            repository.obtenerTodosAsync();
            return null;
        }
    }
}
