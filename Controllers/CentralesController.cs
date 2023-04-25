using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cemsa_BackEnd.Models;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/central")]
    [ApiController]
    public class CentralesController : ControllerBase
    {
        //GET: api/centrales/obtenerCentrales
        /// <summary>
        /// Recupera el listado de Centrales de la Base de datos
        /// </summary>
        /// <returns>Lista de Centrales</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("listaCentrales")]
        public async Task<ActionResult<List<TCentral>>> obtenerCentrales()
        {
            try
            {
                using (var db = new CemsaContext())
                { 
                    var query = await( from tc in db.TCentrals
                               join tec in db.TEstadoCentrals on tc.CenIdEstadoCentral equals tec.EstId
                               join tc2 in db.TClientes on new { tipoDoc = tc.CenTipoDoc, nroDoc = tc.CenNroDoc } equals new { tipoDoc = tc2.CliTipoDoc, nroDoc = tc2.CliNroDoc }
                               join tu in db.TUsuarios on tc2.CliIdUsuario equals tu.UsrId
                               select new
                               {
                                   tc.CenNro,
                                   tc2.CliApeNomDen,
                                   tu.Usuario,
                                   tec.EstDescripcion,
                                   tec.EstId
                               }).ToListAsync();
                    return Ok(query);
                }                  
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Centrales", ex);
            }
        }

        //POST: api/centrales 
        /// <summary>
        /// Modifica estado de una Central
        /// Si es una Baja agrega la Data sino fechaBaja = NULL
        /// </summary>
        /// <returns>Modifica Estado de una Central</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("{cenNum}/{idEstado}")]
        public async Task<ActionResult> modificarEstado(int cenNum, int idEstado)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    var central = await db.TCentrals.FirstOrDefaultAsync(c => c.CenNro == cenNum);
                    if (central != null)
                    {
                        central.CenIdEstadoCentral = idEstado;
                        central.CenFechaBaja = central.CenIdEstadoCentral == 4 ? DateTime.Now : null;
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
                throw new Exception("Error al intentar actualizar los datos del Estado de la Central", ex);
            }
        }

        //GET: api/centrales/obtenerEstados
        /// <summary>
        /// Recupera el listado de Estados de una Central de la Base de datos
        /// </summary>
        /// <returns>Lista de Estados de una Central</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("listaEstadosCentrales")]
        public async Task<List<TEstadoCentral>> obtenerEstados()
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    return await db.TEstadoCentrals.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Estados de Central", ex);
            }
        }
    }
}
