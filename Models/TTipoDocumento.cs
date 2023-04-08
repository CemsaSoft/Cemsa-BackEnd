using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TTipoDocumento
    {
        public TTipoDocumento()
        {
            TClientes = new HashSet<TCliente>();
        }

        public int TdId { get; set; }
        public string TdDescripcion { get; set; } = null!;

        public virtual ICollection<TCliente> TClientes { get; set; }
    }
}
