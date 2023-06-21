using System.ComponentModel.DataAnnotations;

namespace Cemsa_BackEnd.Models
{
    public partial class TAlarma
    {
        public TAlarma()
        {
        }
        //[Key]
        /// <summary>
        /// ID de Alarma
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int AlmId { get; set; }

        /// <summary>
        /// ID de Medicion
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int AlmIdMedicion { get; set; }

        /// <summary>
        /// Mensaje
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "El campo {0} no debe tener {1} caracteres")]
        public string? AlmMensaje { get; set; } = null!;

        /// <summary>
        /// Fecha de Alta de la Alarma
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime AlmFechaHoraBD { get; set; }

        /// <summary>
        /// Visto
        /// </summary>
        public bool AlmVisto { get; set; }
        /// <summary>
        /// Notificadoo
        /// </summary>
        public bool AlmNotificado { get; set; }

        //public virtual Tmedicion? Medicion { get; set; } //= null!;
    }
}
