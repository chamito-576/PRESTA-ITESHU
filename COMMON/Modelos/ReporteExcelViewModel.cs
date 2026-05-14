using COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class ReporteExcelViewModel
    {
        public string Laboratorio { get; set; }

        public string Carrera { get; set; }

        public DateTime? FechaSolicitud { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        public string DescripcionMaterial { get; set; }

        public int NumeroInventario { get; set; }

        public int Cantidad { get; set; }

        public string Observaciones { get; set; }

        public string NombreAdministrador { get; set; }
        public string NombreArchivo { get; set; }

        public string RutaArchivo { get; set; }
    }
}
