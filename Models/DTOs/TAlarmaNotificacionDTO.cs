namespace Cemsa_BackEnd.Models.DTOs
{
    public class TAlarmaNotificacionDTO
    {
        public int NotiAlmId { get; set; }
        public string NotiAlmCentral { get; set; }
        public int NotiAlmServicio { get; set; }
        public string NotiAlmMensaje { get; set; } = null!;
        public DateTime AlmFechaHoraBD { get; set; }
        public string CliEmail { get; set; }
        public string CliApeNomDen { get; set; }

    }
}
