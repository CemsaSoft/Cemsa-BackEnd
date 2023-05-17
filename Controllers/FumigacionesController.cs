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


        //GET: api/fumigaciones
        /// <summary>
        /// Recupera el listado de Fumigaciones de la Base de datos
        /// </summary>
        /// <returns>Lista de fumigaciones</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<List<TFumigacion>> obtenerFumigaciones()
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return await db.TFumigaciones.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Fumigaciones", ex);
            }
        }
        
   

        //GET: api/fumigaciones/id
        /// <summary>
        /// Recupera el fumigacion con el ID pasado por parámetro.
        /// </summary>
        /// <param name="id">ID del fumigacion</param>
        /// <returns>Fumigacion</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id:int}")]
        public async Task<TFumigacion?> obtenerFumigacionesPorId(int id)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return await db.TFumigaciones.FirstOrDefaultAsync(a => a.FumId == id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Fumigacion por ID", ex);
            }
        }
        /*
                                //Dudas aunque no sea un metodo.Me dice que FumObservacion puede ser nulo por eso hay un error
                               //lO HICE POR OBSERVACION QUE NO LE VEO SENTIDO
                //GET: api/fumigaciones/busquedaDescripcion
                /// <summary>
                /// Recupera el listado de Fumigaciones de la Base de datos con la descripción solicitada. 
                /// </summary>
                /// <param name="busquedaDescripcion">Descripción del fumigacion</param>
                /// <returns>Fumigacion</returns>
                /// <exception cref="Exception"></exception>
                [HttpGet("{busquedaDescripcion}")]
                public async Task<ActionResult<List<TFumigacion>>> obtenerSFumigacionesPorDescr(string busquedaDescripcion)
                {
                    try
                    {
                        using (var db = new CemsaContext())
                        {
         // duda esta aca                   return await db.TFumigaciones.Where(x => x.FumObservacion.Contains(busquedaDescripcion)).ToListAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al intentar obtener Fumigacion por Descripcion", ex);
                    }
                }     */

              

        //POST: api/fumigaciones/registrarFumigacion
        /// Registra un fumigacion a la Base de Datos x datos Body
        /// </summary>
        /// <param name="fumigacion">Fumigacion a registrar</param>
        /// <returns>Fumigacion registrado</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TFumigacion fumigacion)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    db.TFumigaciones.Add(fumigacion);
                    await db.SaveChangesAsync();
                    return Ok(fumigacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar insertar los datos de una Fumigacion", ex);
            }
        }
          
            //POST: api/fumigaciones/registrarFumigacion
                /// <summary>
                /// Registra un fumigacion a la Base de Datos
                /// </summary>
                /// <param name="nroCentral">Numero de la Central</param>
                /// <param name="observacion">Observacion de la Fumigacion</param>
                /// <returns>Fumigacion registrado</returns>
                /// <exception cref="Exception"></exception>
                [HttpPost("{observacion}")]
                public async Task<ActionResult> registrarFumigacion(string observacion)
                {
                    try
                    {
                        using (var db = new CemsaContext())                     //Comparar
                        {
                            TFumigacion fumigacion = new TFumigacion();
                            fumigacion.FumObservacion = observacion;
                            db.TFumigaciones.Add(fumigacion);
                            await db.SaveChangesAsync();
                            return Ok();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al intentar insertar los datos de una Fumigacion, ingresando la observación ", ex);
                    }
                }

        
                //POST: api/fumigaciones/modificarFumigacion
                /// Agregar un fumigacion a la Base de Datos x datos Descripcion y Unidad x parametros
                /// </summary>
                /// <param name="id">id del Fumigacion a modificar</param>
                /// <param name="fumigacion">Fumigacion a modificar</param>
                /// <returns>Se modifico la fumigacion</returns>
                /// <exception cref="Exception"></exception>
                [HttpPost("{id:int}")]
                public async Task<ActionResult> modificarFumigacion(int id, TFumigacion fumigacion)
                {
                    try
                    {
                        using (var db = new CemsaContext())
                        {
                            if (fumigacion.FumId != id)
                            {
                                return BadRequest("El Id de la Fumigacion no esta registrado en el sistema");
                            }
                            db.TFumigaciones.Update(fumigacion);
                            await db.SaveChangesAsync();
                            return Ok();
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
                [HttpDelete("{id}")]
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
