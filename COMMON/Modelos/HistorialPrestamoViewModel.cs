using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class HistorialPrestamoViewModel
    {
        public int IdPrestamo { get; set; }

        public string Material { get; set; }

        public DateTime FechaEntrega { get; set; }

        public string Laboratorio { get; set; }

        public string Estado { get; set; }
    }
}
