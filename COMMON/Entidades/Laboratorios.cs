using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Entidades
{
    public class Laboratorios: CamposControl
    {
        public int IdLaboratorio { get; set; }
        public string Nombre { get; set; }
        public string Edificio { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
        public int? IdUsuario { get; set; }
    }
}
