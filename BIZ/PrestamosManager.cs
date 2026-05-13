using COMMON.Entidades;
using COMMON.Modelos;
using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class PrestamosManager:GenericManager<Prestamos>
    {
        public PrestamosManager(AbstractValidator<Prestamos> validador) : base(validador)
        {
        }
        public async Task<List<Prestamos>> ObtenerPrestamosUsuario(int idUsuario)
        {
            try
            {
                var solicitudes =
                    await FabricManager.SolicitudesManager
                    .ObtenerTodos();

                var prestamos =
                    await ObtenerTodos();

                var prestamosUsuario =
                    prestamos.Where(p =>
                        solicitudes.Any(s =>
                            s.IdSolicitud == p.IdSolicitud &&
                            s.IdUsuario == idUsuario) &&
                        p.Estado == "Aprobado")
                    .ToList();

                return prestamosUsuario;
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return null;
            }
        }

        public async Task<List<PrestamosQRViewModel>>BuscarPorQR(string codigoQR,int idLaboratorio)
        {
            try
            {
                HttpResponseMessage response =
                    await _httpClient.GetAsync(
                    $"api/Prestamos/BuscarQR?codigoQR={codigoQR}&idLaboratorio={idLaboratorio}");

                var content =
                    await response
                    .Content
                    .ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Error = "";

                    return JsonConvert
                        .DeserializeObject
                        <List<PrestamosQRViewModel>>
                        (content);
                }
                else
                {
                    Error = content;

                    return null;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return null;
            }
        }
    }
}
