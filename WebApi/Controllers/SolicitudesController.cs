using COMMON.Entidades;
using DAL;
using COMMON.Modelos;
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

        [HttpGet]
        [Route("ObtenerSolicitudesAdmin/{idUsuario}")]
        public ActionResult ObtenerSolicitudesAdmin(
    int idUsuario)
        {
            try
            {
                var repo =
                    (DBSqlServer<Solicitudes>)
                    Parametros.FabricaRepository
                    .SolicitudesRepository();

                var lista =
                    repo.ObtenerSolicitudesAdmin(
                        idUsuario);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("CambiarEstadoSolicitud")]
        public ActionResult CambiarEstadoSolicitud( 
            [FromBody]
        CambiarEstadoViewModel model)
        {
            try
            {
                var repo =
                    (DBSqlServer<Solicitudes>)
                    Parametros.FabricaRepository
                    .SolicitudesRepository();

                var resultado =
                    repo.CambiarEstadoSolicitud(
                        model.IdSolicitud,
                        model.Estado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
