using COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMMON.Interfaces
{
    /// <summary>
    /// Interfaz que define el comportamiento de una clase para la comunicación con una Base de datos
    /// </summary>
    /// <typeparam name="T">Tipo de clase a implementar</typeparam>
    public interface IDB<T> where T : CamposControl
    {
        /// <summary>
        /// Obtiene el error, si es que existe, al ejecutar alguna operación
        /// </summary>
        string Error { get; }
        /// <summary>
        /// Obtiene todos los registros de la tabla
        /// </summary>
        /// <returns>Lista de objetos que representan los registros</returns>
        List<T> ObtenerTodos();
        /// <summary>
        /// Obtiene el registro correspondiente al Id proporcionado
        /// </summary>
        /// <param name="id">Id del registro a obtener</param>
        /// <returns>Objeto correspondiente al Id proporcionado</returns>
        T ObtenerPorId(int id);
        T ObtenerPorId(string id);
        bool Eliminar(T entidad);
        T Insertar(T entidad);
        T Actualizar(T entidad);
        List<M> EjecutaProcedimiento<M>(string nombre, Dictionary<string, string> parametros) where M : class;

    }
}
