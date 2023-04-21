using Cemsa_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/servicios")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        //GET: api/servicios
        /// <summary>
        /// Recupera el listado de Servicios de la Base de datos
        /// </summary>
        /// <returns>Lista de servicios</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<List<TServicio>> obtenerServicios()
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return await db.TServicios.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Servicios", ex);
            }
        }

        //GET: api/servicios/id
        /// <summary>
        /// Recupera el servicio con el ID pasado por parámetro.
        /// </summary>
        /// <param name="id">ID del servicio</param>
        /// <returns>Servicio</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id:int}")]
        public async Task<TServicio?> obtenerServiciosPorId(int id)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return await db.TServicios.FirstOrDefaultAsync(a => a.SerId == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Servicio por ID", ex);
            }
        }

        //GET: api/servicios/busquedaDescripcion
        /// <summary>
        /// Recupera el listado de Servicios de la Base de datos con la descripción solicitada. 
        /// </summary>
        /// <param name="busquedaDescripcion">Descripción del servicio</param>
        /// <returns>Servicio</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{busquedaDescripcion}")]
        public async Task<ActionResult<List<TServicio>>> obtenerServiciosPorDescr(string busquedaDescripcion)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return await db.TServicios.Where(x => x.SerDescripcion.Contains(busquedaDescripcion)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Servicio por Descripcion", ex);
            }
        }

        //POST: api/servicios/registrarServicio
        /// Registra un servicio a la Base de Datos x datos Body
        /// </summary>
        /// <param name="servicio">Servicio a registrar</param>
        /// <returns>Servicio registrado</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TServicio servicio)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    db.TServicios.Add(servicio);
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar insertar los datos de un Servicio", ex);
            }
        }

        //POST: api/servicios/registrarServicio
        /// <summary>
        /// Registra un servicio a la Base de Datos
        /// </summary>
        /// <param name="descripcion">Descripción del Servicio</param>
        /// <param name="unidad">Unidad de Medida del Servicio</param>
        /// <returns>Servicio registrado</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("{descripcion}/{unidad}")]
        public async Task<ActionResult> registrarServicio(string descripcion, string unidad)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    TServicio servicio = new TServicio();
                    servicio.SerDescripcion = descripcion;
                    servicio.SerUnidad = unidad;
                    db.TServicios.Add(servicio);
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar insertar los datos de un Servicio, ingresando descripcion y unidad ", ex);
            }
        }

        //POST: api/servicios/modificarServicio
        /// Agregar un servicio a la Base de Datos x datos Descripcion y Unidad x parametros
        /// </summary>
        /// <param name="id">id del Servicio a modificar</param>
        /// <param name="servicio">Servicio a modificar</param>
        /// <returns>Se modifico el servicio</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("{id:int}")]
        public async Task<ActionResult> modificarServicio(int id, TServicio servicio)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    if (servicio.SerId != id)
                    {
                        return BadRequest("El Id del Servicio no esta registrado en el sistema");
                    }
                    db.TServicios.Update(servicio);
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar los datos de un Servicio", ex);
            }
        }

        //DELETE: api/servicios/id
        /// <summary>
        /// Eliminar un servicio a la Base de Datos
        /// </summary>
        /// <param name="id">id del Servicio a eliminar</param>
        /// <returns>Se elimino el servicio</returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{id}")]
        public async Task<ActionResult> eliminarServicio(int id)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    var servicioEliminar = db.TServicios.Find(id);
                    if (servicioEliminar == null)
                    {
                        return BadRequest("El Id del Servicio no esta registrado en el sistema");
                    }
                    var serviciosXCentral = db.TServiciosxcentrals.Where(s => s.SxcNroServicio == id).ToList();
                    if (serviciosXCentral.Any())
                    {
                        return BadRequest("No se puede eliminar el Servicio ya que está vinculado a una o varias Centrales");
                    }
                    db.TServicios.Remove(servicioEliminar);
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar eleminar un Servicio", ex);
            }
        }
    }
}
