using COMMON.Entidades;
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
    }
}
