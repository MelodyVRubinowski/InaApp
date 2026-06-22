    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using inaApp.DTOs.Cliente;
using System.Threading.Tasks;
using inaApp.Common.Exceptions;
using inaApp.Common.interfaces;
using inaApp.Entities;
using inaApp.Repository;
using AutoMapper;
using inaApp.Common.Response;

namespace inaApp.Services
{
    public class ClienteService : IGenericService<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>
    {
        private readonly IGenericRepository<Cliente> _clienteRepo;
        private readonly IMapper _mapper;

        public ClienteService(IGenericRepository<Cliente> clienteRepo, IMapper mapper)
        {
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        public async Task<Response<ClienteResponseDTO>> CrearAsync(ClienteCreateDTO entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Nombre))
                throw new RequiredFieldException("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(entity.PrimerApellido))
                throw new RequiredFieldException("El primer apellido es requerido");

            if (string.IsNullOrWhiteSpace(entity.NumeroIdentificacion))
                throw new RequiredFieldException("El número de identificación es requerido");

            if (!string.IsNullOrWhiteSpace(entity.CorreoElectronico))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(entity.CorreoElectronico, emailPattern))
                    throw new InvalidEmailException($"El correo '{entity.CorreoElectronico}' no es válido");
            }

            if (!string.IsNullOrWhiteSpace(entity.Telefono))
            {
                if (!entity.Telefono.All(char.IsDigit))
                    throw new InvalidPhoneException($"El teléfono '{entity.Telefono}' solo debe contener números");
            }

            var clientes = await _clienteRepo.obtenerTodosAsync();
            if (clientes.Any(c => c.TipoIdentificacion == entity.TipoIdentificacion &&
                                  c.NumeroIdentificacion == entity.NumeroIdentificacion))
            {
                throw new DuplicateClientException($"Ya existe un cliente con identificación {entity.NumeroIdentificacion}");
            }

            var cliente = _mapper.Map<Cliente>(entity);
            cliente = await _clienteRepo.CrearAsync(cliente);
            var clienteResponse = _mapper.Map<ClienteResponseDTO>(cliente);

            return new Response<ClienteResponseDTO>
            {
                Data = clienteResponse,
                Message = "Cliente creado exitosamente",
                Success = true
            };
        }

        public async Task<Response<ClienteResponseDTO>> ActualizarAsync(ClienteUpdateDTO entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Nombre))
                throw new RequiredFieldException("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(entity.PrimerApellido))
                throw new RequiredFieldException("El primer apellido es requerido");

            if (string.IsNullOrWhiteSpace(entity.NumeroIdentificacion))
                throw new RequiredFieldException("El número de identificación es requerido");

            if (!string.IsNullOrWhiteSpace(entity.CorreoElectronico))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(entity.CorreoElectronico, emailPattern))
                    throw new InvalidEmailException($"El correo '{entity.CorreoElectronico}' no es válido");
            }

            if (!string.IsNullOrWhiteSpace(entity.Telefono))
            {
                if (!entity.Telefono.All(char.IsDigit))
                    throw new InvalidPhoneException($"El teléfono '{entity.Telefono}' solo debe contener números");
            }

            var clientes = await _clienteRepo.obtenerTodosAsync();
            if (clientes.Any(c => c.TipoIdentificacion == entity.TipoIdentificacion &&
                                  c.NumeroIdentificacion == entity.NumeroIdentificacion &&
                                  c.Id != entity.Id))
            {
                throw new DuplicateClientException($"Ya existe un cliente con identificación {entity.NumeroIdentificacion}");
            }

            var existe = await _clienteRepo.obtenerPorIdAsync(entity.Id);
            if (existe is null)
                throw new NotFoundException($"El cliente con el id {entity.Id} no existe");

            _mapper.Map(entity, existe);
            existe = await _clienteRepo.ActualizarAsync(existe);
            var clienteResponse = _mapper.Map<ClienteResponseDTO>(existe);

            return new Response<ClienteResponseDTO>
            {
                Data = clienteResponse,
                Message = "Cliente actualizado exitosamente",
                Success = true
            };
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            var cliente = await _clienteRepo.obtenerPorIdAsync(id);
            if (cliente is null)
                throw new NotFoundException($"El cliente con el id {id} no existe");

            var eliminado = await _clienteRepo.EliminarAsync(id);

            return new Response<bool>
            {
                Data = eliminado,
                Message = eliminado ? "Cliente eliminado exitosamente" : "Error al eliminar el cliente",
                Success = eliminado
            };
        }

        public async Task<Response<ClienteResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var cliente = await _clienteRepo.obtenerPorIdAsync(id);

            if (cliente is null)
                throw new NotFoundException($"El cliente con el id {id} no existe");

            var clienteResponse = _mapper.Map<ClienteResponseDTO>(cliente);

            return new Response<ClienteResponseDTO>
            {
                Data = clienteResponse,
                Message = "Cliente encontrado",
                Success = true
            };
        }

        public async Task<Response<List<ClienteResponseDTO>>> ObtenerTodosAsync()
        {
            var clientes = await _clienteRepo.obtenerTodosAsync();
            var listaDTOs = _mapper.Map<List<ClienteResponseDTO>>(clientes);

            return new Response<List<ClienteResponseDTO>>
            {
                Data = listaDTOs,
                Message = "Lista de clientes obtenida exitosamente",
                Success = true
            };
        }
    }
}