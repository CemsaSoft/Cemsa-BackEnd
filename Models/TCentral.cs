using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Models
{
    public partial class TCentral
    {
        public TCentral()
        {
            TFumigacions = new HashSet<TFumigacion>();
            TServiciosxcentrals = new HashSet<TServiciosxcentral>();
        }

        /// <summary>
        /// ID Central Servicio
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CenNro { get; set; }

        /// <summary>
        /// Numero Imei
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 15, MinimumLength = 15, ErrorMessage = "El campo {0} no debe tener {1} caracteres")]
        public string CenImei { get; set; } = null!;

        /// <summary>
        /// Numero de la Coordenada en X de la Central
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? CenCoorX { get; set; }

        /// <summary>
        /// Numero de la Coordenada en Y de la Central
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? CenCoorY { get; set; }

        /// <summary>
        /// Fecha de Alta de la Central
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime CenFechaAlta { get; set; }
        
        /// <summary>
        /// Fecha de Baja de la Central
        /// </summary>
        public DateTime? CenFechaBaja { get; set; }

        /// <summary>
        /// ID del estado de la Central
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CenIdEstadoCentral { get; set; }

        /// <summary>
        /// Tipo de Documento del Cliente de la Central
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CenTipoDoc { get; set; }

        /// <summary>
        /// Número de Documento del Cliente de la Central
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CenNroDoc { get; set; } = null!;

        public virtual TCliente? Cen { get; set; } //= null!;
        public virtual TEstadoCentral? CenIdEstadoCentralNavigation { get; set; } //= null!;
        public virtual ICollection<TFumigacion>? TFumigacions { get; set; }
        public virtual ICollection<TServiciosxcentral>? TServiciosxcentrals { get; set; }
    }
}
