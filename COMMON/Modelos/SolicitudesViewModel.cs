using COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class SolicitudesViewModel:Solicitudes
    {
        public string NombreUsuario { get; set; }

        public string Matricula { get; set; }

        public string Carrera { get; set; }

        public string Semestre { get; set; }
        public string NombreMaterial { get; set; }
    }
}
