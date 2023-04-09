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
        [HttpGet]
        public List<TServicio> obtenerServicios()
        {
            using (var db = new CemsaContext())
            {
                return db.TServicios.ToList();
            }
        }

        // GET api/servicio
        [HttpGet("{id:int}")]
        public TServicio? obtenerServiciosPorId (int id)
        {

            using (var db = new CemsaContext())
            {
                return db.TServicios.Include(a => a.SerId).FirstOrDefault(a => a.SerId == id);
                 
            }
            //var servicio =  CemsaContext.TServicios.AnyAsync<TServicio>(x => x.SerDescripcion == nombre);

        }

        // POST api/<ServiciosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ServiciosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServiciosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
