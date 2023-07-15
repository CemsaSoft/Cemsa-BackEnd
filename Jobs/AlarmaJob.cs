using Cemsa_BackEnd.Models.DTOs;
using Cemsa_BackEnd.Models;
using Cemsa_BackEnd;
using Newtonsoft.Json;
using Quartz;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.EntityFrameworkCore;
using Cemsa_BackEnd.Helpers.Mail;

namespace Scoring.API.Jobs
{
    [DisallowConcurrentExecution]
    public class AlarmaJob : IJob
    {
        private readonly HelperMail _helperMail;

        private HttpWebRequest _httpWebRequest;

        public AlarmaJob(IConfiguration configuration, HelperMail helperMail)
        {

            Configuration = configuration;
            _helperMail = helperMail;
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
                            if(!await _helperMail.enviarAlarmaMail(noti))
                            {
                                return false;
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
