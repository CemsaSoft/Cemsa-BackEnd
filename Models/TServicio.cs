using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TServicio
    {
        public TServicio()
        {
            TServiciosxcentrals = new HashSet<TServiciosxcentral>();
        }

        public int SerId { get; set; }
        public string SerDescripcion { get; set; } = null!;
        public string SerUnidad { get; set; } = null!;

        public virtual ICollection<TServiciosxcentral> TServiciosxcentrals { get; set; }
    }
}
