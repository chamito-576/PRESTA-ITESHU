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
    public class InventarioManager:GenericManager<Inventario>    
    {
        public InventarioManager(AbstractValidator<Inventario> validador) : base(validador)
        {
        }

        public async Task<ExpedienteInventarioModel> ObtenerExpedienteInventario(int idLaboratorio)
        {
            try
            {
                HttpResponseMessage response =
                    await _httpClient.GetAsync(
                        $"api/Inventario/" +
                        $"ObtenerExpedienteInventario/" +
                        $"{idLaboratorio}");

                var content =await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ExpedienteInventarioModel>(content);
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
