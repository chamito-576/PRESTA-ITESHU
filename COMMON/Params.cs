using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON
{
    public static class Params
    {
        public static string UsuarioConectado = "UsuarioAPI";
        public static string RolUsuarioConectado { get; set; }
        public static int IdLaboratorioConectado { get; set; }
        public static int IdUsuarioConectado { get; set; }
        //public static string UrlAPI = @"http://apipresta.runasp.net/";
        public static string UrlAPI = @"https://localhost:7223/";
    }
}
