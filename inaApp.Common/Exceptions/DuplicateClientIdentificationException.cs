using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Exceptions
{
    public class DuplicateClientIdentificationException:Exception
    {

        public DuplicateClientIdentificationException()
        {
            
        }

        public DuplicateClientIdentificationException(string? message):base(message)
        {
            
        }
    }
}
