using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Entidades
{
    public class Reportes : CamposControl
    {
        public int IdReporte { get; set; }
        public int IdPrestamo { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string? RutaPDF { get; set; }
    }
}
