using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class Tmedicion
    {
        public int MedId { get; set; }
        public int MedNro { get; set; }
        public int MedSer { get; set; }
        public decimal? MedValor { get; set; }
        public DateTime MedFechaHoraSms { get; set; }
        public DateTime? MedFechaHoraBd { get; set; }
        public string? MedObservacion { get; set; }

        public virtual TServiciosxcentral Med { get; set; } 
    }
}
