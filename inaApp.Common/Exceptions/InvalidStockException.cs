using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class invalidStockException : Exception
    {
        public invalidStockException()
        {
        }

        public invalidStockException(string? message) : base(message)
        {
        }
    }
}
