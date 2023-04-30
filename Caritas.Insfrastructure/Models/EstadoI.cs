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
    public class EstadoI : BaseDomainModel
    {
        [Column(TypeName = "varchar(1)")]
        public string? Codigo { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Nombre { get; set; }
    }
}
