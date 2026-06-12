using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class BusinessValidationException: Exception
    {
        public BusinessValidationException() { }

        public BusinessValidationException(string message) : base(message)
        {
        }
    }
}
