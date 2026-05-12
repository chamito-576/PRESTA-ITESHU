using COMMON.Entidades;
using COMMON.Modelos;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : GenericController<Usuarios>
    {
        public UsuariosController() : base(Parametros.FabricaRepository.UsuariosRepository())
        {
        }

        // LOGIN ADMIN
        [HttpPost]
        [Route("LoginAdmin")]
        public ActionResult LoginAdmin(
            [FromBody] LoginViewModel login)
        {
            try
            {
                var repo =
                    (DBSqlServer<Usuarios>)
                    Parametros.FabricaRepository
                    .UsuariosRepository();

                var usuario =
                    repo.LoginAdmin(
                        login.Correo,
                        login.Contrasena);

                if (usuario != null)
                {
                    return Ok(usuario);
                }

                return BadRequest(
                    "Correo o contraseña incorrectos");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("LoginUsuario")]
        public ActionResult LoginUsuario(
        [FromBody] LoginViewModel login)
        {
            try
            {
                var repo =
                    (DBSqlServer<Usuarios>)
                    Parametros.FabricaRepository
                    .UsuariosRepository();

                var usuario =
                    repo.LoginUsuario(
                        login.Correo,
                        login.Contrasena);

                if (usuario != null)
                {
                    return Ok(usuario);
                }

                return BadRequest(
                    "Correo o contraseña incorrectos");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
