using COMMON.Entidades;
using COMMON.Interfaces;
using COMMON.Validadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FabricRepository
    {
        private string _cadenaConexion;
        private TipoBD _tipo;
        public FabricRepository(string cadenaConexion, TipoBD tipo)
        {
            _cadenaConexion = cadenaConexion;
            _tipo = tipo;
        }

        public IDB<Inventario> InventarioRepository()
        {
            switch (_tipo)
            {
                case TipoBD.SQLServer:
                    return new DBSqlServer<Inventario>(_cadenaConexion, new InventarioValidator(), "IdMaterial", true);
                case TipoBD.MySql:
                    return new DBMySql<Inventario>(_cadenaConexion, new InventarioValidator(), "IdMaterial", true);
                default:
                    return null;
            }

        }

        public IDB<Laboratorios> LaboratoriosRepository()
        {
            switch (_tipo)
            {
                case TipoBD.SQLServer:
                    return new DBSqlServer<Laboratorios>(_cadenaConexion, new LaboratoriosValidator(), "IdLaboratorio", true);
                case TipoBD.MySql:
                    return new DBMySql<Laboratorios>(_cadenaConexion, new LaboratoriosValidator(), "IdLaboratorio", true);
                default:
                    return null;
            }

        }

        public IDB<Prestamos> PrestamosRepository()
        {
            switch (_tipo)
            {
                case TipoBD.SQLServer:
                    return new DBSqlServer<Prestamos>(_cadenaConexion, new PrestamosValidator(), "IdPrestamo", true);
                case TipoBD.MySql:
                    return new DBMySql<Prestamos>(_cadenaConexion, new PrestamosValidator(), "IdPrestamo", true);
                default:
                    return null;
            }

        }

        public IDB<Reportes> ReportesRepository()
        {
            switch (_tipo)
            {
                case TipoBD.SQLServer:
                    return new DBSqlServer<Reportes>(_cadenaConexion, new ReportesValidator(), "IdReporte", true);
                case TipoBD.MySql:
                    return new DBMySql<Reportes>(_cadenaConexion, new ReportesValidator(), "IdReporte", true);
                default:
                    return null;
            }

        }
        public IDB<Solicitudes> SolicitudesRepository()
        {
            switch (_tipo)
            {
                case TipoBD.SQLServer:
                    return new DBSqlServer<Solicitudes>(_cadenaConexion, new SolicitudesValidator(), "IdSolicitud", true);
                case TipoBD.MySql:
                    return new DBMySql<Solicitudes>(_cadenaConexion, new SolicitudesValidator(), "IdSolicitud", true);
                default:
                    return null;
            }
        }

        public IDB<Usuarios> UsuariosRepository()
        {
            switch (_tipo)
            {
                case TipoBD.SQLServer:
                    return new DBSqlServer<Usuarios>(_cadenaConexion, new UsuariosValidator(), "IdUsuario", true);
                case TipoBD.MySql:
                    return new DBMySql<Usuarios>(_cadenaConexion, new UsuariosValidator(), "IdUsuario", true);
                default:
                    return null;
            }

        }

    }

    public enum TipoBD
    {
        SQLServer,
        MySql
    }
}
