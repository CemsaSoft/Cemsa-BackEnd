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
        public List<TServicio> obtenerServicios()
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return db.TServicios.ToList();
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
        public TServicio? obtenerServiciosPorId(int id)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return db.TServicios.FirstOrDefault(a => a.SerId == id);
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
        public ActionResult<List<TServicio>> obtenerServiciosPorDescr(string busquedaDescripcion)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return db.TServicios.Where(x => x.SerDescripcion.Contains(busquedaDescripcion)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Servicio por Descripcion", ex);
            }
        }

        /// Agregar un servicio a la Base de Datos x datos Body
        [HttpPost]
        public ActionResult Post([FromBody] TServicio servicio)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    db.TServicios.Add(servicio);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar insertar los datos de un Servicio", ex);
            }
        }

        //POST
        /// <summary>
        /// Registra un servicio a la Base de Datos
        /// </summary>
        /// <param name="descripcion">Descripción del Servicio</param>
        /// <param name="unidad">Unidad de Medida del Servicio</param>
        /// <returns>Servicio registrado</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("{descripcion}/{unidad}")]
        public ActionResult registrarServicio(string descripcion, string unidad)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    TServicio servicio = new TServicio();
                    servicio.SerDescripcion = descripcion;
                    servicio.SerUnidad = unidad;
                    db.TServicios.Add(servicio);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar insertar los datos de un Servicio, ingresando descripcion y unidad ", ex);
            }
        }

        /// Agregar un servicio a la Base de Datos x datos Descripcion y Unidad x parametros
        [HttpPost("{id:int}")]
        public ActionResult modificarServicio(int id, TServicio servicio)
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
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar los datos de un Servicio", ex);
            }
        }

        /// Eliminar un servicio a la Base de Datos
        [HttpDelete("{id}")]
        public ActionResult EliminarServicio(int id)
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
                    db.TServicios.Remove(servicioEliminar);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar eleminar un Servicio", ex);
            }
        }

        //// DELETE api/<ServiciosController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        // POST api/<ServiciosController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ServiciosController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}
    }
}
