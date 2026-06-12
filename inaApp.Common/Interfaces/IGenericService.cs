using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IGenericService<E>
    {
        //Firmas de los metodos
        Task<List<E>> obtenerTodosAsync();//Task xq son metodos asincronicos

        Task<E> ObtenerPorIdAsync(int id);

        Task<E> CrearAsync(E entity);

        Task<E> ActualizarAsync(int id,E entity);

        Task<bool> EliminarAsync(int id);
    }
}
