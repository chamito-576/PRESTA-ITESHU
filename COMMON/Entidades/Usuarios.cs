using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Entidades
{
    public class Usuarios: CamposControl
    {
        public int IdUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string? Matricula { get; set; }
        public string? Carrera { get; set; }
        public string? Semestre { get; set; }
        public string Rol { get; set; }
        public int IdLaboratorio { get; set; }
        public bool Activo { get; set; }
    }
}
