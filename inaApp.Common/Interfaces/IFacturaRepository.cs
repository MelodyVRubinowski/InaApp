using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IFacturaRepository<T> : IGenericRepository<T>
    {
        Task AnularAsync(T factura);
    }
}