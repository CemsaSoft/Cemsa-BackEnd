using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Cemsa_BackEnd.Models;
using Cemsa_BackEnd.Models.DTOs;

namespace Cemsa_BackEnd.Helpers.Mail
{
    public class HelperMail
    {
        private readonly string _smtpServer = "smtp-relay.sendinblue.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "5k4cemsa2021@gmail.com";
        private readonly string _smtpPassword = "TDLXCzdOAjrtfIcW";

        private readonly IWebHostEnvironment env;

        public HelperMail(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public async Task<bool> enviarAlarmaMail(TAlarmaNotificacionDTO noti)
        {
            string path = env.ContentRootPath;
            path = Path.Combine(path, "Helpers", "Mail", "Templates", "AlarmMail.html");
            string htmlPlano = obtenerPlantilla(path);
            string pBodyHtml = htmlPlano.Replace("{{0}}", (noti.CliApeNomDen))
                                        .Replace("{{1}}", (noti.NotiAlmCentral.ToString()))
                                        .Replace("{{2}}", (noti.NotiAlmServicio))
                                        .Replace("{{3}}", (noti.AlmFechaHoraBD.ToString()))
                                        .Replace("{{4}}", (noti.NotiAlmMensaje));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Cemsa", _smtpUsername));
            message.To.Add(new MailboxAddress(noti.CliApeNomDen, noti.CliEmail));
            message.Subject = "Alerta de medición con valor: " + noti.NotiMedValor;
            message.Body = new TextPart("html")
            {
                Text = pBodyHtml
            };

            return await SendEmail(message);
        }

        public async Task<bool> enviarResetPasswordMail(string usuario, string mail)
        {
            string path = env.ContentRootPath;
            path = Path.Combine(path, "Helpers", "Mail", "Templates", "ResetPasswordMail.html");
            string htmlPlano = obtenerPlantilla(path);
            string pBodyHtml = htmlPlano.Replace("{{0}}", (usuario));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Cemsa", _smtpUsername));
            message.To.Add(new MailboxAddress(usuario, mail));
            message.Subject = "Recuperar Contraseña";
            message.Body = new TextPart("html")
            {
                Text = pBodyHtml
            };
            return await SendEmail(message);
        }

        private async Task <bool> SendEmail(MimeMessage message)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string obtenerPlantilla(string path)
        {
            try
            {
                StreamReader archivo = new StreamReader(path, false);
                string contenido = archivo.ReadToEnd();
                archivo.Close();

                return contenido;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
