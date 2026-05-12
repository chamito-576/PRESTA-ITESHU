using COMMON.Entidades;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIZ
{
    public class UsuariosManager : GenericManager<Usuarios>
    {
        public UsuariosManager(AbstractValidator<Usuarios   > validador) : base(validador)
        {
        }
        public async Task<Usuarios> LoginAdmin(string correo,string contrasena)
        {
            try
            {
                var login = new
                {
                    Correo = correo,
                    Contrasena = contrasena
                };

                var json =
                    JsonConvert.SerializeObject(login);

                var body =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response =
                    await _httpClient.PostAsync(
                        "api/Usuarios/LoginAdmin",
                        body);

                var content =
                    await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert
                        .DeserializeObject<Usuarios>(content);
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

        public async Task<Usuarios> LoginUsuario(string correo,string contrasena)
        {
            try
            {
                var login = new
                {
                    Correo = correo,
                    Contrasena = contrasena
                };

                var json =
                    JsonConvert.SerializeObject(login);

                var body =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");

                HttpResponseMessage response =
                    await _httpClient.PostAsync(
                        "api/Usuarios/LoginUsuario",
                        body);

                var content =
                    await response.Content
                    .ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert
                        .DeserializeObject<Usuarios>(content);
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
    }
}
