using inaApp.Common.Interfaces;
using inaApp.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class EmpleadoRespository : IGenericRepository<Empleado>
    {
        public Task<Empleado> ActualizarAsync(int id, Empleado entity)
        {
            throw new NotImplementedException();
        }

        public bool validarNombreRepetido(string nombre)
        {
            throw new NotImplementedException();
        }

        Task<Empleado> IGenericRepository<Empleado>.CrearAsync(Empleado entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericRepository<Empleado>.EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<Empleado> IGenericRepository<Empleado>.ObtenerPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Empleado>> IGenericRepository<Empleado>.obtenerTodosAsync()
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericRepository<Empleado>.validarNombreRepetido(string nombre)
        {
            throw new NotImplementedException();
        }
    }
}
