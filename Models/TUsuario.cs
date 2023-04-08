using System;
using System.Collections.Generic;

namespace Cemsa_BackEnd.Models
{
    public partial class TUsuario
    {
        public TUsuario()
        {
            TClientes = new HashSet<TCliente>();
        }

        public int UsrId { get; set; }
        public string Usuario { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<TCliente> TClientes { get; set; }
    }
}
