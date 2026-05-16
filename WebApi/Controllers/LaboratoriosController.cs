using COMMON.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoriosController : GenericController<Laboratorios>
    {   
        public LaboratoriosController() : base(Parametros.FabricaRepository.LaboratoriosRepository())
        {
        }
    }
}
