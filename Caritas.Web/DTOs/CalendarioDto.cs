using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Caritas.Web.DTOs
{
    public class CalendarioDto
    {
        public int Id { get; set; }

        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        public DateTime Vencimiento { get; set; }

        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        public DateTime VencimientoProc { get; set; }

        [Column(TypeName = "smalldatetime")]
        [DataType(DataType.Date)]
        public DateTime FechaAPartir { get; set; }
        public string Observacion { get; set; }
        public bool Registros { get; set; }
    }
}
