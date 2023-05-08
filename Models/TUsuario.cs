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

        /// <summary>
        /// identificador incremental del registro de usuario en la BD
        /// </summary>
        public int UsrId { get; set; }

        /// <summary>
        /// nombre de usuario
        /// </summary>
        public string Usuario { get; set; } = null!;

        /// <summary>
        /// contraseña de usuario
        /// </summary>
        public string Password { get; set; } = null!;

        public virtual ICollection<TCliente> TClientes { get; set; }
    }
}
