using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TEstadoserviciosxCentral
    {
        public TEstadoserviciosxCentral()
        {
            TServiciosxcentrals = new HashSet<TServiciosxcentral>();
        }

        public int EstId { get; set; }
        public string EstDescripcion { get; set; } = null!;

        public virtual ICollection<TServiciosxcentral> TServiciosxcentrals { get; set; }
    }
}
