using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TServiciosxcentral
    {
        public TServiciosxcentral()
        {
            Tmedicions = new HashSet<Tmedicion>();
        }

        public int SxcNroCentral { get; set; }
        public int SxcNroServicio { get; set; }
        public int SxcEstado { get; set; }
        public DateTime SxcFechaAlta { get; set; }
        public DateOnly? SxcFechaBaja { get; set; }

        public virtual TEstadoserviciosxCentral? SxcEstadoNavigation { get; set; } // = null!;
        public virtual TCentral? SxcNroCentralNavigation { get; set; } // = null!;
        public virtual TServicio? SxcNroServicioNavigation { get; set; } // = null!;
        public virtual ICollection<Tmedicion>? Tmedicions { get; set; }
    }
}
