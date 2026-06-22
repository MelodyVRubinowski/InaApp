using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inaApp.Common.interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class ClienteRepository : IGenericRepository<Cliente>
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> CrearAsync(Cliente entity)
        {
            try
            {
                _context.Cliente.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Cliente> ActualizarAsync(Cliente entity)
        {
            try
            {
                _context.Cliente.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var cliente = await obtenerPorIdAsync(id);
                if (cliente == null)
                    return false;

                cliente.Activo = false;
                _context.Cliente.Update(cliente);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Cliente> obtenerPorIdAsync(int id)
        {
            try
            {
                return await _context.Cliente
                    .Where(x => x.Id == id && x.Activo == true)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Cliente>> obtenerTodosAsync()
        {
            try
            {
                return await _context.Cliente
                    .AsNoTracking()
                    .Where(x => x.Activo == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}