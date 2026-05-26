using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Modelos
{
    public class ExpedienteInventarioModel
    {
        public string Laboratorio { get; set; }

        public DateTime FechaGeneracion { get; set; }

        public int TotalInventario { get; set; }

        public int Disponibles { get; set; }

        public int Prestados { get; set; }

        public int Devueltos { get; set; }

        public int Retrasados { get; set; }

        public int TotalMovimientos { get; set; }

        public List<DetalleMovimientoModel>
            Movimientos
        { get; set; }
    }
}
