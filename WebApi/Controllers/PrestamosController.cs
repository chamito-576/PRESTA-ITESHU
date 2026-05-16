using COMMON.Entidades;
using BIZ;
using DAL;
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
        [HttpGet]
        [Route("BuscarPrestamoQR/{idPrestamo}/{idLaboratorio}")]
        public ActionResult BuscarPrestamoQR(int idPrestamo, int idLaboratorio)
        {
            try
            {
                var repo =
                    (DBSqlServer<Prestamos>)
                    Parametros.FabricaRepository
                    .PrestamosRepository();

                var lista =
                    repo.BuscarPrestamoQR(
                        idPrestamo,
                         idLaboratorio);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
