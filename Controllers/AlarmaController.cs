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

/*using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;*/
using Cemsa_BackEnd.Models.DTOs;

namespace Cemsa_BackEnd.Controllers
{
    [Route("api/alarma")]
    [ApiController]
    [Authorize]
    public class AlarmaController : ControllerBase
    {

        /*private readonly string _smtpServer = "";//"smtp-relay.sendinblue.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "";//"5k4cemsa2021@gmail.com";
        private readonly string _smtpPassword = "";//"TDLXCzdOAjrtfIcW";*/


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
        /*[HttpGet]
        [Route("notificarAlarmas")]
        public async Task<ActionResult> notificarAlarmas()
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<TAlarma> alarmas = await db.TAlarmas.Where(x => x.AlmNotificado == false).ToListAsync();
                    List<TAlarmaNotificacionDTO> alarmasNotificar = await (from a in db.TAlarmas
                                                              join med in db.Tmedicions on a.AlmIdMedicion equals med.MedId
                                                              join c in db.TCentrals on med.MedNro equals c.CenNro
                                                              join cl in db.TClientes on
                                                              new { tipoDoc = c.CenTipoDoc, nroDoc = c.CenNroDoc } equals new { tipoDoc = cl.CliTipoDoc, nroDoc = cl.CliNroDoc }
                                                              join s in db.TServicios on med.MedSer equals s.SerId
                                                              where a.AlmNotificado == false
                                                              select new TAlarmaNotificacionDTO
                                                              {
                                                                  NotiAlmId= a.AlmId,
                                                                  NotiAlmCentral = c.CenNro,
                                                                  NotiAlmServicio = s.SerDescripcion,
                                                                  NotiAlmMensaje = a.AlmMensaje,
                                                                  NotiMedValor = med.MedValor,
                                                                  AlmFechaHoraBD = a.AlmFechaHoraBD,
                                                                  CliEmail = cl.CliEmail,
                                                                  CliApeNomDen = cl.CliApeNomDen
                                                              }).ToListAsync();

                    foreach (TAlarmaNotificacionDTO noti in alarmasNotificar)
                    {
                        if(noti.CliEmail != null)
                        {
                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("Cemsa", _smtpUsername));
                            message.To.Add(new MailboxAddress(noti.CliApeNomDen, noti.CliEmail));
                            message.Subject = "Alerta de medición con valor: " + noti.NotiMedValor ;
                            message.Body = new TextPart("plain")
                            {
                                Text = "Estimado Cliente: " + noti.CliApeNomDen + ",\n\nSe notifica sobre la siguiente alarma en la central " + noti.NotiAlmCentral +
                                " en el servicio " + noti.NotiAlmServicio +  " con fecha " + noti.AlmFechaHoraBD + ".\n\n" + noti.NotiAlmMensaje +
                                "\r\n\nDesde el equipo de Cemsa, le enviamos nuestros saludos.\r\n"
                            };
                            using (var client = new SmtpClient())
                            {
                                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                            TAlarma alarma = alarmas.Find(match: x => x.AlmId == noti.NotiAlmId);
                            if (alarma != null)
                            {
                                alarma.AlmNotificado = true;
                                db.TAlarmas.Update(alarma);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                        return Ok("Se notificaron las alarmas y se actualizó el estado de las mismas.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Failed to send email: {ex.Message}");
                }
            }
        }*/
    }
}
