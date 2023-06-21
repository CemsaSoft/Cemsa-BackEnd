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
    [Route("api/alarma")]
    [ApiController]
    [Authorize]
    public class AlarmaController : ControllerBase
    {


        //GET: api/alarmas
        /// <summary>
        /// Recupera el listado de alarmas de la Base de datos y Modifica estado de visto de la alarma
        /// </summary>
        /// <returns>Lista de Alarma y Modifica estado de visto de la alarma</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("obtenerAlarmasClienteModificaEstado/{idUsuario}")]
        public async Task<ActionResult<List<TAlarma>>> obtenerAlarmasClienteModificaEstado(int idUsuario)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from a in db.TAlarmas
                                       join med in db.Tmedicions on a.AlmIdMedicion equals med.MedId
                                       join c in db.TCentrals on med.MedNro equals c.CenNro
                                       join cl in db.TClientes on
                                       new { tipoDoc = c.CenTipoDoc, nroDoc = c.CenNroDoc } equals new { tipoDoc = cl.CliTipoDoc, nroDoc = cl.CliNroDoc }
                                       join u in db.TUsuarios on cl.CliIdUsuario equals u.UsrId
                                       join s in db.TServicios on med.MedSer equals s.SerId
                                       where u.UsrId == idUsuario
                                       select new
                                       {
                                           a.AlmId,
                                           a.AlmIdMedicion,
                                           c.CenNro,
                                           s.SerDescripcion,
                                           a.AlmMensaje,
                                           med.MedValor,
                                           a.AlmFechaHoraBD,
                                           a.AlmVisto,
                                           a.AlmNotificado,
                                       }).ToListAsync();

                        var query2 = await (from a in db.TAlarmas
                                           join med in db.Tmedicions on a.AlmIdMedicion equals med.MedId
                                           join c in db.TCentrals on med.MedNro equals c.CenNro
                                           join cl in db.TClientes on new { tipoDoc = c.CenTipoDoc, nroDoc = c.CenNroDoc } equals new { tipoDoc = cl.CliTipoDoc, nroDoc = cl.CliNroDoc }
                                           join u in db.TUsuarios on cl.CliIdUsuario equals u.UsrId
                                           join s in db.TServicios on med.MedSer equals s.SerId
                                           where u.UsrId == idUsuario
                                           select a).ToListAsync();
                        foreach (var alarma in query2)
                        {
                            alarma.AlmVisto = true;
                        }

                        await db.SaveChangesAsync();
                    return Ok(query);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Configs Alarma y Actualizar estado", ex);
            }           
        }

        //GET: api/alarmas
        /// <summary>
        /// Recupera el listado de alarmas de la Base de datos y Modifica estado de visto de la alarma
        /// </summary>
        /// <returns>Lista de Alarma y Modifica estado de visto de la alarma</returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("obtenerCantAlarmasCliente/{idUsuario}")]
        public async Task<ActionResult<List<TAlarma>>> obtenerCantAlarmasCliente(int idUsuario)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var query = await (from a in db.TAlarmas
                                       join med in db.Tmedicions on a.AlmIdMedicion equals med.MedId
                                       join c in db.TCentrals on med.MedNro equals c.CenNro
                                       join cl in db.TClientes on
                                       new { tipoDoc = c.CenTipoDoc, nroDoc = c.CenNroDoc } equals new { tipoDoc = cl.CliTipoDoc, nroDoc = cl.CliNroDoc }
                                       join u in db.TUsuarios on cl.CliIdUsuario equals u.UsrId
                                       join s in db.TServicios on med.MedSer equals s.SerId
                                       where u.UsrId == idUsuario && a.AlmVisto == false
                                       select new
                                       {
                                           a.AlmId,
                                           a.AlmIdMedicion,
                                           c.CenNro,
                                           s.SerDescripcion,
                                           a.AlmMensaje,
                                           med.MedValor,
                                           a.AlmFechaHoraBD,
                                           a.AlmVisto,
                                           a.AlmNotificado,
                                       }).ToListAsync();
                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar obtener Lista de Configs Alarma", ex);
            }
        }

    }
}
