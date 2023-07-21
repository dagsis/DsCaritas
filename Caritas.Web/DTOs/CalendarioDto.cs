using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Caritas.Web.DTOs
{
    public class CalendarioDto
    {
        public int Id { get; set; }
        public string Periodo { get; set; }
        public string Cuota { get; set; }
        public string Anio { get; set; }

        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        public DateTime Vencimiento { get; set; }
    }
}
