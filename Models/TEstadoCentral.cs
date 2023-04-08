using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TEstadoCentral
    {
        public TEstadoCentral()
        {
            TCentrals = new HashSet<TCentral>();
        }

        public int EstId { get; set; }
        public string EstDescripcion { get; set; } = null!;

        public virtual ICollection<TCentral> TCentrals { get; set; }
    }
}
