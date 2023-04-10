using Cemsa_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        /// Listar Servicios de la Base de datos
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

        /// Listar Servicios de la Base de datos por Id
        [HttpGet("{id:int}")]
        public TServicio? obtenerServiciosPorId(int id)
        {
            try { 
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

        /// Listar Servicios de la Base de datos por Descripcion 
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

        /// Agregar un servicio a la Base de Datos x datos Descripcion y Unidad x parametros
        [HttpPost("{descripcion}/{unidad}")]
        public ActionResult Post(string descripcion, string unidad)
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
        public ActionResult modificarSeervicio(int id, TServicio servicio)
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
