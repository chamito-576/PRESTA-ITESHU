using COMMON.Entidades;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : GenericController<Reportes>
    {
        public ReportesController() : base(Parametros.FabricaRepository.ReportesRepository())
        {
        }
        [HttpGet("ObtenerReporteExcel")]
        public ActionResult ObtenerReporteExcel()
        {
            try
            {
                var resultado =
                    Parametros
                    .FabricaRepository
                    .PrestamosRepository();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
