using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Entidades
{
    public class Solicitudes:CamposControl
    {
        public int IdSolicitud { get; set; }
        public int IdUsuario { get; set; }
        public int IdMaterial { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; }
    }
}
