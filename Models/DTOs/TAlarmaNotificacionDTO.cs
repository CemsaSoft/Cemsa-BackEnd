using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Cemsa_BackEnd.Models.DTOs
{
    public class TAlarmaNotificacionDTO
    {
        public int NotiAlmId { get; set; }
        public int NotiAlmCentral { get; set; }
        public string NotiAlmServicio { get; set; }
        public string NotiAlmMensaje { get; set; } = null!;
        public decimal? NotiMedValor { get; set; }
        public DateTime AlmFechaHoraBD { get; set; }
        public string? CliEmail { get; set; }
        public string CliApeNomDen { get; set; }

    }
}
