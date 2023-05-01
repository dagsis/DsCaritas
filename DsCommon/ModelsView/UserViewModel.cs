using DsCommon.Atributes;
using DsCommon.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DsCommon.ModelsView
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Column(TypeName = "varchar(80)")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public string PhoneNumber { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public string CPostal { get; set; }
        public EstadoEnum Estado { get; set; } = EstadoEnum.Activo;

        [NotMapped]
        public string Status { get; set; } = "1";

        [NotMapped]
        public string Role { get; set; }

        //   public IEnumerable<SelectListItem> Roles { get; set; }

        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} y un máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "La contraseña y la confirmación son distintas.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public int CompaniaId { get; set; }
       // public List<SelectListItem> UserClaims { get; set; }
    }
}
