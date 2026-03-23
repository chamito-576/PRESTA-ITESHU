using COMMON.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : GenericController<Prestamos>
    {
        public PrestamosController() : base(Parametros.FabricaRepository.PrestamosRepository())
        {
        }
    }
}
