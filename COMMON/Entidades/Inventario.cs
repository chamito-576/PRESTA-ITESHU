using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Entidades
{
    public class Inventario:CamposControl
    {
        public int IdMaterial { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int IdLaboratorio { get; set; }
        public bool Activo { get; set; }
    }
}
