using inaApp.Common.Enums;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services
{
    public class ClienteService : IGenericService<Cliente>
    {
        private readonly IGenericRepository<Cliente> _clienteRepo;

        public ClienteService(IGenericRepository<Cliente> clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<Cliente> CrearAsync(Cliente cliente)
        {
            //Validar si el cliente viene nulo
            if (cliente == null)
            {
                throw new BusinessValidationException("Los datos del cliente son obligatorios.");
            }

            //Validar si el nombre esta vacío o es nulo
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new BusinessValidationException("El nombre del cliente es obligatorio.");
            }

            //Validar si el numero de identificación esta vacío o es nulo
            if (string.IsNullOrWhiteSpace(cliente.NumeroIdentificacion))
            {
                throw new BusinessValidationException("El número de identificación es obligatorio.");
            }

            //Validar si el correo viene nulo o vacio
            if (string.IsNullOrWhiteSpace(cliente.CorreoElectronico))
            {
                throw new BusinessValidationException("El correo electrónico es obligatorio.");
            }

            //Validar si el enum es permitido
            if (!Enum.IsDefined(typeof(TipoIdentificacionEnum), (TipoIdentificacionEnum)cliente.IdTipoIdentificacion))
            {
                throw new BusinessValidationException("El tipo de identificación no es permitido");
            }

            //Pasamos el tipo de identificación
            cliente.TipoIdentificacion = (TipoIdentificacionEnum)cliente.IdTipoIdentificacion;

            return await _clienteRepo.CrearAsync(cliente);
        }

        public async Task<Cliente> ActualizarAsync(int id, Cliente cliente)
        {
            //Validar si el id es permitido
            if (id <= 0)
            {
                throw new BusinessValidationException("El id del cliente no es válido.");
            }

            //Validar si el cliente no es nulo
            if (cliente == null)
            {
                throw new BusinessValidationException("Los datos del cliente son obligatorios.");
            }

            //Validar si el nombre es nulo o viene vacio
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
            {
                throw new BusinessValidationException("El nombre del cliente es obligatorio.");
            }

            //Validar si el correo viene nulo o vacio
            if (string.IsNullOrWhiteSpace(cliente.CorreoElectronico))
            {
                throw new BusinessValidationException("El correo electrónico es obligatorio.");
            }

            //Validar si el enum es permitido
            if (!Enum.IsDefined(typeof(TipoIdentificacionEnum), (TipoIdentificacionEnum)cliente.IdTipoIdentificacion))
            {
                throw new BusinessValidationException("El tipo de identificación no es permitido");
            }

            //Le pasamos el tipo de identificación
            cliente.TipoIdentificacion = (TipoIdentificacionEnum)cliente.IdTipoIdentificacion;

            return await _clienteRepo.ActualizarAsync(id, cliente);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            //Validar si el id es permitdo
            if (id <= 0)
            {
                throw new BusinessValidationException("El id del cliente no es válido.");
            }

            return await _clienteRepo.EliminarAsync(id);
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            //Valiar si id es permitido
            if (id <= 0)
            {
                throw new BusinessValidationException("El id del cliente no es válido.");
            }

            return await _clienteRepo.ObtenerPorIdAsync(id);
        }

        public async Task<List<Cliente>> obtenerTodosAsync()
        {
            return await _clienteRepo.obtenerTodosAsync();
        }
    }
}
