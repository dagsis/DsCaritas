using DsCommon.Atributes;
using DsCommon.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caritas.Web.DTOs
{
    public class RoleDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Column(TypeName = "varchar(80)")]
        [PrimeraLetraMayuscula]
        public string Name { get; set; }
        public EstadoEnum Estado { get; set; } = EstadoEnum.Activo;

        [NotMapped]
        public string Status { get; set; } = "1";

        public int CompaniaId { get; set; }
    }
}
