using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Entidades
{
    public class Prestamos:CamposControl
    {
        public int IdPrestamo { get; set; }
        public int IdSolicitud { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string Estado { get; set; }
        public string CodigoQR { get; set; }
        public string? Observaciones { get; set; }
    }
}
