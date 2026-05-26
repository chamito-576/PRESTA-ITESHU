using COMMON.Entidades;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : GenericController<Inventario>
    {
        public InventarioController() : base(Parametros.FabricaRepository.InventarioRepository())
        {
        }
        [HttpGet]
        [Route("ObtenerExpedienteInventario/{idLaboratorio}")]
        public ActionResult ObtenerExpedienteInventario(int idLaboratorio)
        {
            try
            {
                var repo =(DBSqlServer<Inventario>)Parametros.FabricaRepository.InventarioRepository();

                var expediente =repo.ObtenerExpedienteInventario(idLaboratorio);

                return Ok(expediente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
