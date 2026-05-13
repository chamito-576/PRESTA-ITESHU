using COMMON.Entidades;
using COMMON.Modelos;
using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class SolicitudesManager:GenericManager<Solicitudes>
    {
        public SolicitudesManager(AbstractValidator<Solicitudes> validador) : base(validador)
        {
        }
        public async Task<List<SolicitudesViewModel>>ObtenerSolicitudesAdmin(int idUsuario)
        {
            try
            {
                HttpResponseMessage response =
                    await _httpClient.GetAsync(
                        $"api/Solicitudes/ObtenerSolicitudesAdmin/{idUsuario}");

                var content =
                    await response.Content
                    .ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject
                        <List<SolicitudesViewModel>>(content);
                }

                Error = content;

                return null;
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return null;
            }
        }

        public async Task<bool>CambiarEstadoSolicitud(int idSolicitud,string estado)
        {
            try
            {
                var datos = new
                {
                    IdSolicitud = idSolicitud,
                    Estado = estado
                };

                var json =
                    JsonConvert.SerializeObject(datos);

                var body =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response =
                    await _httpClient.PutAsync(
                        "api/Solicitudes/CambiarEstadoSolicitud",
                        body);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                Error =
                    await response.Content
                    .ReadAsStringAsync();

                return false;
            }
            catch (Exception ex)
            {
                Error = ex.Message;

                return false;
            }
        }
    }
}
