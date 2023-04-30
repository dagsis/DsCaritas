
using Caritas.Insfrastructure.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caritas.Insfrastructure.Model
{
    public class Adjudicado : BaseDomainModel
    {
        public int Nicho { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaAdjudicacion { get; set; }
        public int MesesMantenimiento { get; set; }
        public int Cliente { get; set; }
        public int Titular { get; set; }
        public int Cotitular { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Importe { get; set; }
        public int Empresa { get; set; }
        public int Reserva { get; set; }
        public DateTime FecDesde { get; set; }
        public DateTime FecHasta { get; set; }
        public DateTime FecUltMant { get; set; }
        public DateTime FecUltPago { get; set; }
        public int At { get; set; }
        public int Re { get; set; }
        public int Ce { get; set; }

    }
}
