using Cemsa_BackEnd.Models.DTOs;
using Cemsa_BackEnd.Models;
using Cemsa_BackEnd;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using Quartz;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.EntityFrameworkCore;

namespace Scoring.API.Jobs
{
    [DisallowConcurrentExecution]
    public class AlarmaJob : IJob
    {
        private readonly string _smtpServer = "smtp-relay.sendinblue.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "5k4cemsa2021@gmail.com";
        private readonly string _smtpPassword = "TDLXCzdOAjrtfIcW";


        private HttpWebRequest _httpWebRequest;
        //private readonly AppOptions _appOptions = new AppOptions();




        //Logs _logs;


        public AlarmaJob(IConfiguration configuration)
        {


            Configuration = configuration;
            //Configuration.GetSection(AppOptions.SectionName).Bind(_appOptions);



        }
        public IConfiguration Configuration { get; }


        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var response = notificarAlarmas();

            }
            catch (Exception ex)
            { 

                throw;
            }

            return Task.CompletedTask;
        }

        public async Task<bool> notificarAlarmas()
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
                                                                               NotiAlmId = a.AlmId,
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
                        if (noti.CliEmail != null)
                        {
                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("Cemsa", _smtpUsername));
                            message.To.Add(new MailboxAddress(noti.CliApeNomDen, noti.CliEmail));
                            message.Subject = "Alerta de medición con valor: " + noti.NotiMedValor;
                            message.Body = new TextPart("plain")
                            {
                                Text = "Estimado Cliente: " + noti.CliApeNomDen + ",\n\nSe notifica sobre la siguiente alarma en la central " + noti.NotiAlmCentral +
                                " en el servicio " + noti.NotiAlmServicio + " con fecha " + noti.AlmFechaHoraBD + ".\n\n" + noti.NotiAlmMensaje +
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
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
