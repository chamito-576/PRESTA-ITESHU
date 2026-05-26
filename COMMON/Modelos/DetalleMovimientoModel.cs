using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class DetalleMovimientoModel
    {
        public string Material { get; set; }
        public string Descripcion { get; set; }

        public string Usuario { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        public string Estado { get; set; }
    }
}
