using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class InvalidPhoneException : Exception
    {
        public InvalidPhoneException() { }
        public InvalidPhoneException(string? message) : base(message) { }
    }
}
