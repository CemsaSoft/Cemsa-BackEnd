using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TFumigacion
    {
        public int FumId { get; set; }
        public int FumNroCentral { get; set; }
        public DateTime? FumFechaAlta { get; set; }
        public DateTime? FumFechaRealizacion { get; set; }
        public string? FumObservacion { get; set; }

        public virtual TCentral? FumNroCentralNavigation { get; set; }
    }
}
