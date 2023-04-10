using Cemsa_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        // GET: api/servicio
        /// Listar Servicios de la Base de datos
        [HttpGet]
        public List<TServicio> obtenerServicios()
        {
            using (var db = new CemsaContext())
            {
                return db.TServicios.ToList();
            }
        }

        // GET api/servicio
        /// Listar Servicios de la Base de datos por Id
        [HttpGet("{id:int}")]
        public TServicio? obtenerServiciosPorId(int id)
        {

            using (var db = new CemsaContext())
            {
                return db.TServicios.FirstOrDefault(a => a.SerId == id);
            }
        }

        // GET api/servicio
        /// Listar Servicios de la Base de datos por Descripcion 
        [HttpGet("{busquedaDescripcion}")]
        public ActionResult<List<TServicio>> obtenerServiciosPorDescr(string busquedaDescripcion)
        {
            using (var db = new CemsaContext())
            {
                return db.TServicios.Where(x => x.SerDescripcion.Contains(busquedaDescripcion)).ToList();
            }
        }


        // POST api/<ServiciosController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ServiciosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // Post api/clientes/id
        /// Agregar un servicio a la Base de Datos x datos Body
        [HttpPost]
        public void Post([FromBody] TServicio servicio)
        {
            using (var db = new CemsaContext())
            {
                db.TServicios.Add(servicio);
                db.SaveChanges();
            }
            //{
            //    "serDescripcion": "claudio",
            //    "serUnidad": "uniCla"
            //}
        }

        // Post api/clientes/id
        /// Agregar un servicio a la Base de Datos x datos Descripcion y Unidad x parametros
        [HttpPost("{descripcion}/{unidad}")]
        public void Post(string descripcion, string unidad)
        {
            using (var db = new CemsaContext())
            {
                TServicio servicio = new TServicio();
                servicio.SerDescripcion = descripcion;
                servicio.SerUnidad = unidad;
                db.TServicios.Add(servicio);
                db.SaveChanges();
            }
        }

        // DELETE api/<ServiciosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
