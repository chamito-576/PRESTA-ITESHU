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
        [HttpGet("BuscarQR")]
        public ActionResult BuscarQR(string codigoQR,int idLaboratorio)
        {
            try
            {
                var resultado =
                    Parametros
                    .FabricaRepository
                    .PrestamosRepository()
                    .BuscarPrestamoQR(
                        codigoQR,
                        idLaboratorio);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
