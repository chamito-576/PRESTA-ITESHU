using COMMON;
using COMMON.Entidades;
using COMMON.Interfaces;
using COMMON.Modelos;
using FluentValidation;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public class DBSqlServer<T> : IDB<T> where T : CamposControl
    {
        public string Error { get; private set; }
        private string cadenaDeConexion;
        private string campoId;
        private bool esAutonumerico;
        private AbstractValidator<T> validador;
        public DBSqlServer(string cadenaDeConexion, AbstractValidator<T> validador, string campoId, bool esAutonumerico)
        {
            this.cadenaDeConexion = cadenaDeConexion;
            this.campoId = campoId;
            this.esAutonumerico = esAutonumerico;
            Error = "";
            this.validador = validador;
        }

        public T Actualizar(T entidad)
        {
            Error = "";
            try
            {
                entidad.UsuarioMod = Params.UsuarioConectado;
                entidad.FechaMod = DateTime.Now;
                var resultadoValidacion = validador.Validate(entidad);
                if (resultadoValidacion.IsValid)
                {
                    string sql = $"UPDATE {typeof(T).Name} SET {string.Join(",", entidad.GetType().GetProperties().Where(p => p.Name != campoId).Select(p => p.Name + "=@" + p.Name))} WHERE {campoId}=@Id";

                    //UPDATE Usuario SET Nombre=@Nombre, Apellido=@Apellido, Edad=@Edad WHERE Id=@Id

                    Dictionary<string, object> parametros = new Dictionary<string, object>();
                    foreach (var propiedad in entidad.GetType().GetProperties().Where(p => p.Name != campoId))
                    {
                        parametros.Add("@" + propiedad.Name, propiedad.GetValue(entidad) ?? DBNull.Value);
                    }
                    parametros.Add("@Id", entidad.GetType().GetProperty(campoId).GetValue(entidad));
                    if (EjecutarComando(sql, parametros) == 1)
                    {
                        return entidad;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    Error = string.Join(", ", resultadoValidacion.Errors);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public List<M> EjecutaProcedimiento<M>(string nombre, Dictionary<string, string> parametros) where M : class
        {
            using (SqlConnection conexion = new SqlConnection(cadenaDeConexion))
            {
                conexion.Open();
                using (SqlCommand comando = new SqlCommand(nombre, conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var parametro in parametros)
                    {
                        comando.Parameters.AddWithValue(parametro.Key, parametro.Value);
                    }
                    var reader = comando.ExecuteReader();
                    List<M> lista = new List<M>();
                    while (reader.Read())
                    {
                        M entidad = Activator.CreateInstance<M>();
                        foreach (var prop in entidad.GetType().GetProperties())
                        {
                            if (reader[prop.Name] == DBNull.Value)
                            {
                                prop.SetValue(entidad, null);
                            }
                            else
                            {
                                if (reader[prop.Name] is DateTime)
                                {
                                    DateTime value = (DateTime)reader[prop.Name];
                                    prop.SetValue(entidad, value);
                                }
                                else
                                {
                                    prop.SetValue(entidad, reader[prop.Name]);
                                }
                            }
                        }
                        lista.Add(entidad);
                    }
                    return lista;
                }
            }
        }

        public Usuarios LoginAdmin(string correo, string contrasena)
        {
            try
            {
                using (SqlConnection conexion =
                    new SqlConnection(cadenaDeConexion))
                {
                    conexion.Open();

                    SqlCommand cmd =
                        new SqlCommand("Login_Administrador", conexion);

                    cmd.CommandType =
                        System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Correo", correo);

                    cmd.Parameters.AddWithValue("@Contrasena", contrasena);

                    SqlDataReader reader =
                        cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Usuarios usuario = new Usuarios
                        {
                            IdUsuario =
                                Convert.ToInt32(reader["IdUsuario"]),

                            Nombre =
                                reader["Nombre"].ToString(),

                            Correo =
                                reader["Correo"].ToString(),

                            Rol =
                                reader["Rol"].ToString(),

                            IdLaboratorio =Convert.ToInt32(reader["IdLaboratorio"])


                        };

                        return usuario;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public Usuarios LoginUsuario(string correo,string contrasena)
        {
            try
            {
                using (SqlConnection conexion =
                    new SqlConnection(cadenaDeConexion))
                {
                    conexion.Open();

                    SqlCommand cmd =
                        new SqlCommand(
                            "Login_Usuario",
                            conexion);

                    cmd.CommandType =
                        System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@Correo",
                        correo);

                    cmd.Parameters.AddWithValue(
                        "@Contrasena",
                        contrasena);

                    SqlDataReader reader =
                        cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Usuarios usuario =
                            new Usuarios
                            {
                                IdUsuario =
                                    Convert.ToInt32(
                                        reader["IdUsuario"]),

                                Nombre =
                                    reader["Nombre"].ToString(),

                                Correo =
                                    reader["Correo"].ToString(),

                                Rol =
                                    reader["Rol"].ToString()
                            };

                        return usuario;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public List<SolicitudesViewModel> ObtenerSolicitudesAdmin(int idUsuario)
        {
            try
            {
                List<SolicitudesViewModel> lista =
                    new List<SolicitudesViewModel>();

                using (SqlConnection conexion =
                    new SqlConnection(cadenaDeConexion))
                {
                    conexion.Open();

                    SqlCommand cmd =
                        new SqlCommand(
                            "Page_Solicitudes",
                            conexion);

                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@Accion",
                        1);

                    cmd.Parameters.AddWithValue(
                        "@IdUsuario",
                        idUsuario);

                    SqlDataReader reader =
                        cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lista.Add(new SolicitudesViewModel
                        {
                            IdSolicitud =
                                Convert.ToInt32(
                                    reader["IdSolicitud"]),

                            NombreUsuario =
                                reader["NombreUsuario"]
                                .ToString(),

                            NombreMaterial =
                                reader["NombreMaterial"]
                                .ToString(),

                            Estado =
                                reader["Estado"]
                                .ToString(),

                            FechaSolicitud =
                                Convert.ToDateTime(
                                    reader["FechaSolicitud"])
                        });
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return null;
            }
        }

        public List<PrestamosQRViewModel> BuscarPrestamoQR(int idPrestamo,int idLaboratorio)
        {
            try
            {
                using (SqlConnection conexion =
                    new SqlConnection(cadenaDeConexion))
                {
                    conexion.Open();

                    SqlCommand cmd =
                        new SqlCommand(
                            "Buscar_Prestamo_QR",
                            conexion);

                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@IdPrestamo",
                            idPrestamo);

                    cmd.Parameters.AddWithValue(
                        "@IdLaboratorio",
                        idLaboratorio);

                    SqlDataReader reader =
                        cmd.ExecuteReader();

                    List<PrestamosQRViewModel> lista =
                        new List<PrestamosQRViewModel>();

                    while (reader.Read())
                    {
                        lista.Add(
                            new PrestamosQRViewModel
                            {
                                IdPrestamo =
                                    Convert.ToInt32(
                                        reader["IdPrestamo"]),

                                Usuario =
                                    reader["Usuario"].ToString(),

                                Material =
                                    reader["Material"].ToString(),

                                FechaEntrega =
                                    reader["FechaEntrega"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(
                                        reader["FechaEntrega"]),

                                FechaDevolucion =
                                    reader["FechaDevolucion"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(
                                        reader["FechaDevolucion"]),

                                Estado =
                                    reader["Estado"].ToString(),

                                Observaciones =
                                    reader["Observaciones"].ToString()
                            });
                    }

                    return lista;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return null;
            }
        }

        public List<ReporteExcelViewModel> ObtenerReporteExcel()
        {
            try
            {
                Dictionary<string, string> parametros =
                    new Dictionary<string, string>();

                return EjecutaProcedimiento
                    <ReporteExcelViewModel>(
                    "Reporte_Excel_Prestamos",
                    parametros);
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return null;
            }
        }
        public bool CambiarEstadoSolicitud(int idSolicitud,string estado)
        {
            try
            {
                using (SqlConnection conexion =
                    new SqlConnection(cadenaDeConexion))
                {
                    conexion.Open();

                    SqlCommand cmd =
                        new SqlCommand(
                            "Page_Solicitudes",
                            conexion);

                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@Accion",
                        2);

                    cmd.Parameters.AddWithValue(
                        "@IdSolicitud",
                        idSolicitud);

                    cmd.Parameters.AddWithValue(
                        "@Estado",
                        estado);

                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return false;
            }
        }

        

        public bool Eliminar(T entidad)
        {
            Error = "";
            try
            {
                string sql = $"DELETE FROM {typeof(T).Name} WHERE {campoId}=@Id";
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@Id", entidad.GetType().GetProperty(campoId).GetValue(entidad));
                return EjecutarComando(sql, parametros) == 1;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        private int EjecutarComando(string sql, Dictionary<string, object> parametros)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaDeConexion))
            {
                conexion.Open();
                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    foreach (var parametro in parametros)
                    {
                        comando.Parameters.AddWithValue(parametro.Key, parametro.Value);
                    }
                    return comando.ExecuteNonQuery();
                }
            }
        }

        public T Insertar(T entidad)
        {
            Error = "";
            try
            {
                entidad.UsuarioAlta = Params.UsuarioConectado;
                entidad.FechaAlta = DateTime.Now;
                var resultadoValidacion = validador.Validate(entidad);
                if (resultadoValidacion.IsValid)
                {
                    string sql;
                    Dictionary<string, object> parametros = new Dictionary<string, object>();
                    if (esAutonumerico)
                    {
                        sql = $"INSERT INTO {typeof(T).Name} ({string.Join(",", entidad.GetType().GetProperties().Where(p => p.Name != campoId).Select(p => p.Name))}) VALUES ({string.Join(",", entidad.GetType().GetProperties().Where(p => p.Name != campoId).Select(p => "@" + p.Name))})";

                        //INSERT INTO Usuario (Nombre, Apellido, Edad) VALUES (@Nombre, @Apellido, @Edad)

                        foreach (var propiedad in entidad.GetType().GetProperties().Where(p => p.Name != campoId))
                        {
                            parametros.Add("@" + propiedad.Name,propiedad.GetValue(entidad) ?? DBNull.Value);
                        }
                    }
                    else
                    {
                        sql = $"INSERT INTO {typeof(T).Name} ({string.Join(",", entidad.GetType().GetProperties().Select(p => p.Name))}) VALUES ({string.Join(",", entidad.GetType().GetProperties().Select(p => "@" + p.Name))})";

                        foreach (var propiedad in entidad.GetType().GetProperties())
                        {
                            parametros.Add("@" + propiedad.Name,propiedad.GetValue(entidad) ?? DBNull.Value);
                        }
                    }

                    if (EjecutarComando(sql, parametros) == 1)
                    {
                        //inserto
                        if (esAutonumerico)
                        {
                            sql = $"SELECT * FROM {typeof(T).Name} WHERE {campoId}=(Select MAX({campoId}) from {typeof(T).Name})";
                            var consulta = EjecutarConsulta(sql, new Dictionary<string, object>());
                            if (consulta.Count == 1)
                            {
                                return consulta.First();
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return entidad;
                        }
                    }
                    else
                    {
                        //Fallo
                        return null;
                    }

                }
                else
                {
                    Error = string.Join(", ", resultadoValidacion.Errors);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public T ObtenerPorId(int id)
        {
            return ObtenerPorId(id.ToString());
        }

        public T ObtenerPorId(string id)
        {
            try
            {
                string sql = $"SELECT * FROM {typeof(T).Name} where {campoId}=@Id";
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("@Id", id);
                return EjecutarConsulta(sql, parametros).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }

        public List<T> ObtenerTodos()
        {
            Error = "";
            try
            {
                string sql = $"SELECT * FROM {typeof(T).Name}";
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                return EjecutarConsulta(sql, parametros);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return null;
            }
        }


        private List<T> EjecutarConsulta(string sql, Dictionary<string, object> parametros)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaDeConexion))
            {
                conexion.Open();
                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    foreach (var parametro in parametros)
                    {
                        comando.Parameters.AddWithValue(parametro.Key, parametro.Value);
                    }
                    var reader = comando.ExecuteReader();
                    List<T> lista = new List<T>();
                    while (reader.Read())
                    {
                        T entidad = Activator.CreateInstance<T>();
                        foreach (var prop in entidad.GetType().GetProperties())
                        {
                            if (reader[prop.Name] == DBNull.Value)
                            {
                                prop.SetValue(entidad, null);
                            }
                            else
                            {
                                if (reader[prop.Name] is DateTime)
                                {
                                    DateTime value = (DateTime)reader[prop.Name];
                                    prop.SetValue(entidad, value);
                                }
                                else
                                {
                                    prop.SetValue(entidad, reader[prop.Name]);
                                }
                            }
                        }
                        lista.Add(entidad);
                    }
                    return lista;
                }
            }
        }
    }
}
