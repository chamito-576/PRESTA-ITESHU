using COMMON.Entidades;
using COMMON.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericController<T> : ControllerBase where T: CamposControl
    {
        //CRUD
        //Create->Post
        //Read->Get
        //Update->Put
        //Delete->Delete

        protected IDB<T> _repositorio;

        public GenericController(IDB<T> repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        //obtener todos
        public ActionResult<List<T>> Get()
        {
            try
            {
                var datos = _repositorio.ObtenerTodos();
                if (datos != null)
                {
                    return Ok(datos);
                }
                else
                {
                    return BadRequest(_repositorio.Error);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<T> GetById(int id)
        {
            try
            {
                var datos = _repositorio.ObtenerPorId(id);
                if (datos != null)
                {
                    return Ok(datos);
                }
                else
                {
                    return BadRequest(_repositorio.Error);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Insertar un nuevo registro
        [HttpPost]
        public ActionResult<T> Post([FromBody] T entidad)
        {
            try
            {
                var datos = _repositorio.Insertar(entidad);
                if (datos != null)
                {
                    return Ok(datos);
                }
                else
                {
                    return BadRequest(_repositorio.Error);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Actualizar un registro
        [HttpPut]
        public ActionResult<T> Put([FromBody] T entidad)
        {
            try
            {
                var datos = _repositorio.Actualizar(entidad);
                if (datos != null)
                {
                    return Ok(datos);
                }
                else
                {
                    return BadRequest(_repositorio.Error);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Eliminar un registro
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                // Primero, intenta obtener la entidad por su ID
                var entidad = _repositorio.ObtenerPorId(id);
                if (entidad == null)
                {
                    return NotFound("Recurso no encontrado."); // Si no se encuentra la entidad
                }

                // Luego, pasa la entidad al método Eliminar
                var resultado = _repositorio.Eliminar(entidad);
                if (resultado)
                {
                    return NoContent(); // Respuesta 204 si la eliminación fue exitosa
                }
                else
                {
                    return BadRequest(_repositorio.Error); // Devuelve un error si la eliminación falla
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Devuelve un error en caso de excepciones
            }
        }

        [HttpDelete("string/{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                // Primero, intenta obtener la entidad por su ID
                var entidad = _repositorio.ObtenerPorId(id);
                if (entidad == null)
                {
                    return NotFound("Recurso no encontrado."); // Si no se encuentra la entidad
                }

                // Luego, pasa la entidad al método Eliminar
                var resultado = _repositorio.Eliminar(entidad);
                if (resultado)
                {
                    return NoContent(); // Respuesta 204 si la eliminación fue exitosa
                }
                else
                {
                    return BadRequest(_repositorio.Error); // Devuelve un error si la eliminación falla
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Devuelve un error en caso de excepciones
            }
        }
    }
}
