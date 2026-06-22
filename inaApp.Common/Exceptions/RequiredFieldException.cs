using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class RequiredFieldException : Exception
    {
        public RequiredFieldException() { }
        public RequiredFieldException(string? message) : base(message) { }
    }
}