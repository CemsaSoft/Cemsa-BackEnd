﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cemsa_BackEnd.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/central")]
    [ApiController]
    public class CentralesController : ControllerBase
    {
        //POST: api/central/registrarCentral
        /// <summary>
        /// Regristrar un nueva Central
        /// </summary>
        /// <param name="central"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("registrarCentral/")]
        public async Task<ActionResult> Post(TCentral central)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    db.TCentrals.Add(central);
                    await db.SaveChangesAsync();
                    return Ok(central);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar insertar los datos de la Central", ex);
            }
        }


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
                    var query = await (from tc in db.TCentrals
                                       join tec in db.TEstadoCentrals on tc.CenIdEstadoCentral equals tec.EstId
                                       join tc2 in db.TClientes
                                       on new { tipoDoc = tc.CenTipoDoc, nroDoc = tc.CenNroDoc } equals new { tipoDoc = tc2.CliTipoDoc, nroDoc = tc2.CliNroDoc }
                                       join tu in db.TUsuarios
                                       on tc2.CliIdUsuario equals tu.UsrId                                       
                                       select new
                                       {
                                           tc.CenNro,
                                           tc.CenImei,
                                           tc.CenCoorX,
                                           tc.CenCoorY,
                                           tc.CenFechaAlta,
                                           tc.CenFechaBaja,
                                           tc.CenIdEstadoCentral,
                                           tc2.CliApeNomDen,
                                           tu.Usuario,
                                           tec.EstDescripcion,
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
        //GET: api/centrales/obtenerServicioXCentral
        /// <summary>
        /// Recupera el listado de Servicio que tiene una Central de la Base de datos
        /// </summary>
        /// <returns>Lista de Servicio x Central</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("serviciosXCentral/{cenNum}")]
        public async Task<ActionResult<List<TServicio>>> obtenerServicioXCentral(int cenNum)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    var query = await (from ts2 in db.TServicios
                                        join ts in db.TServiciosxcentrals on ts2.SerId equals ts.SxcNroServicio
                                        join tc in db.TCentrals on ts.SxcNroCentral equals tc.CenNro
                                        where tc.CenNro == cenNum
                                        select new
                                        {
                                            ts2.SerId,
                                            ts2.SerDescripcion,
                                        }).ToListAsync();
                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Servicio de una Central", ex);
            }
        }

        //POST: api/servicios/registrarServicio
        /// Actualizar datos de una Centra a la Base de Datos
        /// </summary>
        /// <param name="TCentral>Central Actualizar</param>
        /// <returns>Central Actualizada</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("actualizarDatosCentral/{cenNum}/{cenImei}/{cenCoorX}/{cenCoorY}")]
        public async Task<ActionResult> actualizarDatosCentral(int cenNum, string cenImei, string cenCoorX, string cenCoorY)
        {
            try
            {
                using (var db = new CemsaContext())
                {
                    var central = await db.TCentrals.FirstOrDefaultAsync(c => c.CenNro == cenNum);
                    if (central != null)
                    {
                        central.CenImei = cenImei;
                        central.CenCoorX = cenCoorX;
                        central.CenCoorY = cenCoorY;                                                
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
                throw new Exception("Error al intentar actualizar datos de una Central", ex);
            }
        }
    }
}
