using Cemsa_BackEnd.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Models
{
    public partial class TServicio
    {
        public TServicio()
        {
            TServiciosxcentrals = new HashSet<TServiciosxcentral>();
        }

        /// <summary>
        /// ID del Servicio
        /// </summary>
        public int SerId { get; set; }

        /// <summary>
        /// Descripción del servicio
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "El campo {0} no debe tener mas de {1} y menos de {2} caracteres")]
        [FirstCapitalUpper]
        public string SerDescripcion { get; set; } = null!;
       
        /// <summary>
        /// Nombre de la Unidad de medida correspondiente al servicio
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 20, MinimumLength = 1, ErrorMessage = "El campo {0} no debe tener mas de {1} y menos de {2} caracteres")]        
        public string SerUnidad { get; set; } = null!;

        /// <summary>
        /// Tipo de grafico del Servicio
        /// </summary>
        public int SerTipoGrafico { get; set; }
        
        public virtual ICollection<TServiciosxcentral> TServiciosxcentrals { get; set; }
    }
}
