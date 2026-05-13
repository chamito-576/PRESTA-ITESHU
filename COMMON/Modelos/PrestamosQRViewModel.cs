using COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class PrestamosQRViewModel:Prestamos
    {
        public string Usuario { get; set; }
        public string Material { get; set; }
    }
}
