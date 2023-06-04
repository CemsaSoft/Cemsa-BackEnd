using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cemsa_BackEnd.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/alarmaConfig")]
    [ApiController]
    [Authorize]
    public class AlarmaConfigController : ControllerBase
    {

        //GET: api/alarmaConfig
        /// <summary>
        /// Recupera el listado de Alarma Config de la Base de datos de una Central
        /// </summary>
        /// <returns>Lista de Alarmas Config de una central</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("obtenerAlarmaConfigDeCentral/{cfgNro}")]
        public async Task<ActionResult<List<TAlarmaConfig>>> obtenerAlarmaConfigDeCentral(int cfgNro)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from ac in db.TAlarmaConfigs
                                       join s in db.TServicios on ac.CfgSer equals s.SerId
                                       where ac.CfgNro == cfgNro
                                       select new
                                       {
                                           ac.CfgId,
                                           ac.CfgNro,
                                           ac.CfgSer,
                                           ac.CfgNombre,
                                           ac.CfgFechaAlta,
                                           ac.CfgFechaBaja,
                                           ac.CfgValorSuperiorA,
                                           ac.CfgValorInferiorA,
                                           ac.CfgObservacion,
                                           s.SerDescripcion,
                                       }).ToListAsync();
                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Alarma Config de una central", ex);
            }
        }

        //POST: api/alarmaConfig/modificarEstado 
        /// <summary>
        /// Modifica estado de una Alarma Config
        /// Si la accion 0, fechaBaja = NULL - Si la accion 1, fechaBaja = NOW()
        /// </summary>
        /// <returns>Modifica Estado de una Alarma Config</returns>
        /// <exception cref="Exception"></exception>        
        [HttpPost("modificarEstado/{accion}/{cfgId}")]
        public async Task<ActionResult> modificarEstado(int accion, int cfgId)

        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var alarmaConf = await db.TAlarmaConfigs.FirstOrDefaultAsync(a => a.CfgId == cfgId);
                    if (alarmaConf != null)
                    {
                        alarmaConf.CfgFechaBaja = accion == 1 ? DateTime.Now : null;
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar estado de Alarma Config", ex);
            }
        }

        //POST: api/alarmaConfig/modificarAlarmaConfig
        /// <summary>
        /// Realiza la actualización de una Alarma Config en la Base de Datos
        /// </summary>
        /// <returns>Realizar Actlizacion de una Alarma Config</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("modificarAlarmaConfig/")]
        public async Task<ActionResult> modificarAlarmaConfig(TAlarmaConfig alarma)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var a = await db.TAlarmaConfigs.FirstOrDefaultAsync(a => a.CfgId == alarma.CfgId);
                    if (a != null)
                    {
                        a.CfgSer = alarma.CfgSer;
                        a.CfgNombre = alarma.CfgNombre;
                        a.CfgValorSuperiorA = alarma.CfgValorSuperiorA;
                        a.CfgValorInferiorA = alarma.CfgValorInferiorA;
                        a.CfgObservacion = alarma.CfgObservacion;
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar actualizar una Alarma Config", ex);
            }
        }

        //POST: api/alarmaConfig/modificarAlarmaConfig
        /// <summary>
        /// Realiza la actualización de una Alarma Config en la Base de Datos
        /// </summary>
        /// <returns>Realizar Actlizacion de una Alarma Config</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("registrarAlarmaConfig/")]
        public async Task<ActionResult> registrarAlarmaConfig(TAlarmaConfig alarma)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    alarma.CfgFechaAlta = DateTime.Now;
                    alarma.CfgFechaBaja = null;
                    db.TAlarmaConfigs.Add(alarma);
                    await db.SaveChangesAsync();
                    return Ok(alarma);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar registrar una Alarma Config", ex);
            }
        }
    }
}
