using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TCentral
    {
        public TCentral()
        {
            TFumigacions = new HashSet<TFumigacion>();
            TServiciosxcentrals = new HashSet<TServiciosxcentral>();
        }

        public int CenNro { get; set; }
        public string CenImei { get; set; } = null!;
        public string? CenCoorX { get; set; }
        public string? CenCoorY { get; set; }
        public DateTime CenFechaAlta { get; set; }
        public DateTime? CenFechaBaja { get; set; }
        public int CenIdEstadoCentral { get; set; }
        public int CenTipoDoc { get; set; }
        public string CenNroDoc { get; set; } = null!;

        public virtual TCliente Cen { get; set; } = null!;
        public virtual TEstadoCentral CenIdEstadoCentralNavigation { get; set; } = null!;
        public virtual ICollection<TFumigacion> TFumigacions { get; set; }
        public virtual ICollection<TServiciosxcentral> TServiciosxcentrals { get; set; }
    }
}
