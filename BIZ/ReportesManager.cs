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
    public class ReportesManager:GenericManager<Reportes>
    {
        public ReportesManager(AbstractValidator<Reportes> validador) : base(validador)
        {
        }
        public async Task<List<ReporteExcelViewModel>>ObtenerReporteExcel()
        {
            try
            {
                HttpResponseMessage response =
                    await _httpClient.GetAsync(
                    "api/Prestamos/ObtenerReporteExcel");

                var content =
                    await response
                    .Content
                    .ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Error = "";

                    return JsonConvert
                        .DeserializeObject
                        <List<ReporteExcelViewModel>>
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
