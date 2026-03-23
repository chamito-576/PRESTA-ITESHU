using COMMON.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public static class FabricManager
    {
        public static InventarioManager InventarioManager => new InventarioManager(new InventarioValidator());
        public static LaboratoriosManager LaboratoriosManager => new LaboratoriosManager(new LaboratoriosValidator());
        public static PrestamosManager PrestamosManager => new PrestamosManager(new PrestamosValidator());
        public static ReportesManager ReportesManager => new ReportesManager(new ReportesValidator());
        public static SolicitudesManager SolicitudesManager => new SolicitudesManager(new SolicitudesValidator());
        public static UsuariosManager UsuariosManager => new UsuariosManager(new UsuariosValidator());

    }

}
