using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entites;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class ClienteRepository : IGenericRepository<Cliente>
    {

        //Inyección de dependecias de contex
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        //Metodo para actualizar un cliente
        public async Task<Cliente> ActualizarAsync(int id, Cliente entity)
        {
            try
            {
                //Actualizamos cliente
                var resul = await _context.Cliente
                    .Where(x => x.IdCliente == id && x.Activo == true)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(c => c.IdTipoIdentificacion, entity.IdTipoIdentificacion)
                        .SetProperty(c => c.NumeroIdentificacion, entity.NumeroIdentificacion)
                        .SetProperty(c => c.Nombre, entity.Nombre)
                        .SetProperty(c => c.PrimerApellido, entity.PrimerApellido)
                        .SetProperty(c => c.SegundoApellido, entity.SegundoApellido)
                        .SetProperty(c => c.CorreoElectronico, entity.CorreoElectronico)
                        .SetProperty(c => c.Telefono, entity.Telefono)
                    );

                //Validamos si atualizo el cliente si la filas afectadas cambiaron
                if (resul <= 0)
                {
                    throw new NotFoundException($"No se encontro un cliente activo con la el id {id}");
                }

                //Retornamos el primer cliente con ese id y que tenga estado true
                return await _context.Cliente
                    .FirstOrDefaultAsync(x => x.IdCliente == id && x.Activo == true);

            }
            catch (DbUpdateException ex) when (
            //Validar cuando ocurran errores de duplicados
            ex.InnerException is SqlException sqlEx && (sqlEx.Number==2601 || sqlEx.Number==2627))
            {
                if (sqlEx.Message.Contains("IX_tbCliente_TipoIdentificacion_NumeroIdentificacion"))
                {
                    throw new DuplicateClientIdentificationException(
                        "Ya existe un cliente con ese tipo y número de identificación."
                    );
                }

                if (sqlEx.Message.Contains("IX_tbCliente_CorreoElectronico"))
                {
                    throw new ClientEmailAlreadyExistsException(
                        "Ya existe un cliente con ese correo electrónico."
                    );
                }

                throw new ConflictException("Ya existe un cliente con datos duplicados.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo para crear un cliente
        public async Task<Cliente> CrearAsync(Cliente cliente)
        {
            try
            {
                //Agregamos el cliente a la tabla
                await _context.Cliente.AddAsync(cliente);

                //Guardmos los cambios
                await _context.SaveChangesAsync();

                //Devolver el cliente
                return cliente;
            }
            catch (DbUpdateException ex) when (
            //Validar cuando ocurran errores de duplicados
            ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                //Error de tipo de Número de identificación duplicado
                if (sqlEx.Message.Contains("IX_tbCliente_TipoIdentificacion_NumeroIdentificacion"))
                {
                    throw new DuplicateClientIdentificationException(
                        "Ya existe un cliente con ese tipo y número de identificación."
                    );
                }

                //Errror de tipo de correo electrónico duplicado
                if (sqlEx.Message.Contains("IX_tbCliente_CorreoElectronico"))
                {
                    throw new ClientEmailAlreadyExistsException(
                        "Ya existe un cliente con ese correo electrónico."
                    );
                }

                throw new ConflictException("Ya existe un cliente con datos duplicados.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo para eliminar un cliente
        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                //Retornamos si hubo fila afectada
                var result = await _context.Cliente
                    .Where(x => x.IdCliente == id && x.Activo == true)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(c => c.Activo, false)
                    ) > 0;

                //Validamos si ninguna fila fue afectada
                if (!result)
                {
                    throw new NotFoundException($"No se encontro ningun cliente activo con el id {id}");
                }

                //Retornamos resultado
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo para obtener un  cliente por su id
        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            try
            {
                //Buscamos y extraemos el cliente
                var cliente = await _context.Cliente.AsNoTracking()
                    .Where(x => x.IdCliente == id && x.Activo == true)
                    .SingleOrDefaultAsync();

                //Validar el cliente
                if (cliente is null)
                {
                    throw new NotFoundException($"No se encontro ningun cliente activo con el id {id}");
                }

                //Retornamos el cliente
                return cliente;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Metodo para obtener todos los clientes
        public async Task<List<Cliente>> obtenerTodosAsync()
        {
            try
            {
                //Extraer todos los clientes de la bd
                return await _context.Cliente.AsNoTracking()
                    .Where(x => x.Activo == true)
                    .ToListAsync();
            }
            catch (Exception )
            {
                throw;
            }
        }

        //Metodo para validar si lo nombres son iguales
        public async Task<bool> validarNombreRepetido(string nombre)
        {
            try
            {
                //Validar si los nombre son iguales
                return await _context.Cliente
                    .AnyAsync(x => x.Nombre == nombre && x.Activo == true);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}