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

        [HttpGet]
        [Route("ObtenerHistorialUsuario/{idUsuario}")]
        public ActionResult ObtenerHistorialUsuario(int idUsuario)
        {
            try
            {
                var repo =
                    (DBSqlServer<Prestamos>)
                    Parametros.FabricaRepository
                    .PrestamosRepository();

                var lista =
                    repo.ObtenerHistorialUsuario(
                        idUsuario);

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ObtenerReportePrestamo/{idPrestamo}")]
        public ActionResult ObtenerReportePrestamo(int idPrestamo)
        {
            try
            {
                var repo =
                    (DBSqlServer<Prestamos>)
                    Parametros
                    .FabricaRepository
                    .PrestamosRepository();

                var reporte =
                    repo.ObtenerReportePrestamo(
                        idPrestamo);

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}
