﻿#region REFERENCIAS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
// Referencias Necesarias Para El Correcto Funcionamiento
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

#endregion

namespace MCEI.SysControlAdmin.EN.Baptisms___EN
{
    public class Baptisms
    {
        #region ATRIBUTOS DE LA ENTIDAD
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Nombre")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El Nombre debe contener solo Letras")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        [Display(Name = "Apellido")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "El Nombre debe contener solo Letras")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Edad Es Requerida")]
        [StringLength(3, ErrorMessage = "Maximo 3 caracteres")]
        [Display(Name = "Edad")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La edad debe contener solo números")]
        public string Age { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Direccion Es Requerida")]
        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Direccion De Residencia")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Telefono Es Requerido")]
        [StringLength(9, ErrorMessage = "Maximo 8 caracteres")]
        [Display(Name = "Telefono")]
        [RegularExpression("^[0-9-]+$", ErrorMessage = "El Telefono debe contener solo números")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Lugar de Trabajo o Estudio Es Requerido")]
        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Lugar de Trabajo o Estudio")]
        [RegularExpression("^[0-9a-zA-ZáéíóúÁÉÍÓÚñÑ/. \\-]+$", ErrorMessage = "Debe contener solo Letras")]
        public string PlaceOfWorkOrStudy { get; set; } = string.Empty;

        [StringLength(9, ErrorMessage = "Maximo 8 caracteres")]
        [Display(Name = "Telefono del Trabajo o Estudio")]
        [RegularExpression("^[0-9-]+$", ErrorMessage = "El Telefono debe contener solo números")]
        public string? WorkOrStudyPhone { get; set; }

        [Required(ErrorMessage = "El Estado Civil Es Requerido")]
        [StringLength(30, ErrorMessage = "Maximo 30 caracteres")]
        [Display(Name = "Estado Civil")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ/]+$", ErrorMessage = "Debe contener solo Letras")]
        public string CivilStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bautismo En Agua Es Requerido")]
        [StringLength(25, ErrorMessage = "Maximo 25 caracteres")]
        [Display(Name = "Bautizmo En Agua")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ, ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string WaterBaptism { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bautizmo Del Espiritu Santo Es Requerido")]
        [StringLength(2, ErrorMessage = "Maximo 2 caracteres")]
        [Display(Name = "Bautizmo Por El Espiritu Santo")]
        public string BaptismOfTheHolySpirit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fecha De Conversion Es Requerido")]
        [Display(Name = "Fecha De Conversion")]
        [DataType(DataType.Date, ErrorMessage = "Por favor, introduce una fecha válida")]
        public DateTime ConversionDate { get; set; } = DateTime.MinValue;

        [Required(ErrorMessage = "Lugar De Conversion Es Requerido")]
        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Lugar De Conversion")]
        public string PlaceOfConversion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Tiempo De Asistir Es Requerido")]
        [StringLength(50, ErrorMessage = "Maximo 50 caracteres")]
        [Display(Name = "Tiempo de Asistir")]
        public string TimeToGather { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nombre Del Pastor Es Requerido")]
        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Nombre Del Pastor")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string PastorsName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Nombre Del Supervisor")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string? SupervisorsName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        [Display(Name = "Nombre Del Lider")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$", ErrorMessage = "Debe contener solo Letras")]
        public string? LeadersName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Zona Es Requerida")]
        [StringLength(1, ErrorMessage = "Maximo 1 caracteres")]
        [Display(Name = "Zona")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debe contener solo números")]
        public string Zone { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Distrito Es Requerido")]
        [StringLength(1, ErrorMessage = "Maximo 1 caracteres")]
        [Display(Name = "Distrito")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debe contener solo números")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Sector Es Requerido")]
        [StringLength(1, ErrorMessage = "Maximo 1 caracteres")]
        [Display(Name = "Sector")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debe contener solo números")]
        public string Sector { get; set; } = string.Empty;

        [Required(ErrorMessage = "La Celula Es Requerida")]
        [StringLength(1, ErrorMessage = "Maximo 1 caracteres")]
        [Display(Name = "Celula")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Debe contener solo números")]
        public string Cell { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Maximo 300 caracteres")]
        [Display(Name = "Comentarios u Observaciones")]
        public string? CommentsOrObservations { get; set; }

        [Display(Name = "Fotografia")]
        public byte[]? ImageData { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime DateModification { get; set; }


        #endregion

        #region ATRIBUTOS NO MAPEABLES
        // Propiedad para formatear la fecha automáticamente
        [NotMapped]
        public string ConversionDateFormatted => ConversionDate.ToString(@"dd/MM/yyyy");
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
    }
}
