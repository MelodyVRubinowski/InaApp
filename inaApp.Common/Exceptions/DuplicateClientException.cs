using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class DuplicateClientException : Exception
    {
        public DuplicateClientException() { }
        public DuplicateClientException(string? message) : base(message) { }
    }
}
