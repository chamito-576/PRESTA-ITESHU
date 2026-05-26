using COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class PrestamosViewModel: Prestamos
    {
        public string Material { get; set; }
        public string Laboratorio { get; set; }
        public string NombreMaterial { get; set; }
    }
}
