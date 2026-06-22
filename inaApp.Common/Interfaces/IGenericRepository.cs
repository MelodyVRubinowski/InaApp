using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.interfaces
{
    public interface IGenericRepository<E>
    {
        Task<List<E>> obtenerTodosAsync();

        Task<E> obtenerPorIdAsync(int id);

        Task<E> CrearAsync(E entity);

        Task<E> ActualizarAsync(E entity);

        Task<bool> EliminarAsync(int id);
    }
}
