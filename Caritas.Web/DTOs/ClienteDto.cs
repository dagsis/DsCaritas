using DsCommon.Atributes;
using DsCommon.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caritas.Web.DTOs
{
    public class ClienteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [PrimeraLetraMayuscula()]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [PrimeraLetraMayuscula()]
        public string Nombre { get; set; }
        public string Domicilio { get; set; }

        [PrimeraLetraMayuscula()]
        public string Localidad { get; set; }
        public string CodigoPostal { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Documento { get; set; }
        public string Dest { get; set; }
        public string EstadoC { get; set; }

        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido")]
        public string Email { get; set; }

        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido")]
        public string Email2 { get; set; }
        public string Celular { get; set; }
        public string Observaciones { get; set; }
        public EstadoEnum Estado { get; set; } = EstadoEnum.Activo;

        [NotMapped]
        public string Status { get; set; } = "1";

        public string FullName => $"{Apellido} {Nombre}";
    }
}
