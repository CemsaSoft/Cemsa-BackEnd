//Ultimo post SOLO SE MODIFIca observacion (ultimo método)

using Cemsa_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/fumigaciones")]
    [ApiController]
    public class FumigacionesController : ControllerBase
    {

        //GET: api/fumigaciones
        /// <summary>
        /// Recupera el listado de Fumigaciones de la Base de datos de una Central
        /// </summary>
        /// <returns>Lista de fumigaciones de una central</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("obtenerFumigacionesDeCentral/{cenNum}")]
        public async Task<ActionResult<List<TFumigacion>>> obtenerFumigacionesDeCentral(int cenNum)                 
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from f in db.TFumigacions
                                       where f.FumNroCentral == cenNum
                                       select new
                                       {
                                           f.FumId,
                                           f.FumFechaAlta,
                                           f.FumFechaRealizacion,
                                           f.FumObservacion
                                       }).ToListAsync();
                    return Ok(query);
                }         
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Fumigaciones de una central", ex);
            }
        }                         

        //POST: api/fumigaciones/registrarFumigacion
        /// <summary>
        /// Registra un fumigacion a la Base de Datos
        /// </summary>
        /// <param name="fumigacion">Fumigacion a registrar</param>
        /// <returns>Fumigacion registrado</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("registrarFumigacion/")]
        public async Task<ActionResult> registrarFumigacion(TFumigacion fumigacion)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    fumigacion.FumFechaAlta = DateTime.Now;
                    db.TFumigacions.Add(fumigacion);
                    await db.SaveChangesAsync();
                    return Ok(fumigacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar insertar los datos de una Fumigacion", ex);
            }
        }        

        //POST: api/fumigaciones/modificarFumigacion
        /// Modifica una fumigacion en la Base de Datos
        /// </summary>
        /// <param name="fumigacion">Fumigacion a modificar</param>
        /// <returns>Se modifico la fumigacion</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("modificarFumigacion/")]
        public async Task<ActionResult> modificarFumigacion(TFumigacion fumigacion)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    var newFumigacion = await db.TFumigaciones.FirstOrDefaultAsync(f => f.FumId == fumigacion.FumId);
                    if (newFumigacion != null)
                    {
                newFumigacion.FumObservacion = fumigacion.FumObservacion;
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("El Id de la Fumigacion no esta registrado en el sistema");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar los datos de una Fumigacion", ex);
            }
        }
        
        //DELETE: api/fumigaciones/id
        /// <summary>
        /// Eliminar un fumigacion a la Base de Datos
        /// </summary>
        /// <param name="id">id del Fumigacion a eliminar</param>
        /// <returns>Se elimino el fumigacion</returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("eliminarFumigacion/{id}")]
        public async Task<ActionResult> eliminarFumigacion(int id)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    var fumigacionEliminar = db.TFumigaciones.Find(id);
                    if (fumigacionEliminar == null)
                    {
                        return BadRequest("El Id de la Fumigacion no esta registrado en el sistema");
                    }
                    db.TFumigaciones.Remove(fumigacionEliminar);
                    await db.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar eleminar una Fumigacion", ex);
            }
        } 
    }
}
