using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Models
{
    public partial class TCliente
    {
        public TCliente()
        {
            TCentrals = new HashSet<TCentral>();
        }

        public int CliTipoDoc { get; set; }
        public string CliNroDoc { get; set; } = null!;
        public int CliIdUsuario { get; set; }
        public DateTime CliFechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string CliApeNomDen { get; set; } = null!;
        public string? CliEmail { get; set; }
        public string? CliTelefono { get; set; }

        public virtual TUsuario CliIdUsuarioNavigation { get; set; } = null!;
        public virtual TTipoDocumento CliTipoDocNavigation { get; set; } = null!;
        public virtual ICollection<TCentral> TCentrals { get; set; }
    }
}
