using COMMON.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesController : GenericController<Solicitudes>
    {
        public SolicitudesController() : base(Parametros.FabricaRepository.SolicitudesRepository())
        {
        }
    }
}
