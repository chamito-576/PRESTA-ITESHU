using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class ReportePrestamoViewModel
    {
        public int IdPrestamo { get; set; }

        public string NombreUsuario { get; set; }

        public string Carrera { get; set; }

        public string Laboratorio { get; set; }

        public string Material { get; set; }

        public int IdMaterial { get; set; }

        public int Cantidad { get; set; }

        public string Observaciones { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public DateTime? FechaDevolucion { get; set; }
    }
}
