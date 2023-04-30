using DsCommon.Atributes;
using DsCommon.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caritas.Web.ModelsView
{
	public class UserEditDto
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

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
