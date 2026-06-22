using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Enums
{
    
    //Lo hice estatico no requiero hacer una instancia en memoria pora acceder a ellos, soloa accedo al enum y ya 
    public static class Enumeradores {

        public enum TipoIdentificacion
        {
            CedulaFisica = 1,
            CedulaJuridica= 2,
            DIMEX = 3,
            NITE = 4,
            Pasaporte = 5
        }
    }
}