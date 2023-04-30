
using Caritas.Insfrastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caritas.Common.Models
{
    public class Inhumado : BaseDomainModel
    {
        public int Nicho { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Apellido { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Nombre { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaInhumacion { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaFallesimiento { get; set; }
        public string? Origen { get; set; }
        public string? EstadoI { get; set; }
    }
}
