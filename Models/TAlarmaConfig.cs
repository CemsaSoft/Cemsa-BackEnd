using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Models
{
    public partial class TAlarmaConfig
    {
        public TAlarmaConfig()
        {
            TAlarmas = new HashSet<TAlarma>();
        }
        [Key]
        /// <summary>
        /// ID Config de Alarma
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CfgId { get; set; }

        /// <summary>
        /// Numero Central
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CfgNro { get; set; }

        /// <summary>
        /// Id de Servicio
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int CfgSer { get; set; }

        /// <summary>
        /// Nombre de la configuración
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, MinimumLength = 0, ErrorMessage = "El campo {0} no debe tener {1} caracteres")]
        public string? CfgNombre { get; set; } 

        /// <summary>
        /// Fecha de Alta de la Config
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime CfgFechaAlta { get; set; }

        /// <summary>
        /// Fecha de Baja de la Config
        /// </summary>
        public DateTime? CfgFechaBaja { get; set; }

        /// <summary>
        /// Valor Superior A para generar alarma
        /// </summary>
        public decimal? CfgValorSuperiorA { get; set; }

        /// <summary>
        /// Valor Inferior A para generar alarma
        /// </summary>
        public decimal? CfgValorInferiorA { get; set; }

        /// <summary>
        /// Observación
        /// </summary>
        public string? CfgObservacion { get; set; } = null!;

        public virtual TServiciosxcentral? Tsc { get; set; } //= null!;
        public virtual ICollection<TAlarma>? TAlarmas { get; set; }
    }
}
