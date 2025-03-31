#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Referencias Necesarias Para El Correcto Funcionamiento
using MCEI.SysControlAdmin.EN.Membership___EN;
using MCEI.SysControlAdmin.EN.Privilege___EN;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

#endregion

namespace MCEI.SysControlAdmin.EN.Server___EN
{
    public class Server
    {
        #region ATRIBUTOS DE LA ENTIDAD
        [Key]
        public int Id { get; set; }

        [ForeignKey("Membership")]
        [Required(ErrorMessage = "La Membresia es Requerida")]
        [Display(Name = "Membresia")]
        public int IdMembership { get; set; }

        [ForeignKey("Privilege")]
        [Required(ErrorMessage = "El Privilegio Es Requerido")]
        [Display(Name = "Privilegio")]
        public int IdPrivilege { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [Display(Name = "Estado")]
        public byte Status { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Fecha de Modificacion")]
        public DateTime DateModification { get; set; }
        #endregion

        #region ATRIBUTOS NO MAPEABLES
        // Propiedad para dar formato a la fecha
        [NotMapped]
        public string DateCreatedFormatted => DateCreated.ToString(@"dd/MM/yyyy");
        [NotMapped]
        public string DateModificationFormatted => DateModification.ToString(@"dd/MM/yyyy");

        // Propiedad para formatear la hora con AM/PM
        [NotMapped]
        public string TimeCreatedFormatted => DateCreated.ToString("hh:mm tt");
        [NotMapped]
        public string TimeModificationFormatted => DateModification.ToString("hh:mm tt");
        #endregion

        public Privilege? Privilege { get; set; } // Propiedadd de Navegacion

        public Membership? Membership { get; set; }// Propiedad de Navegacion
    }
}
