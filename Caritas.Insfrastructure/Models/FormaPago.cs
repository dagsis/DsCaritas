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
    public class FormaPago : BaseDomainModel
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string? Codigo { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string? Nombre { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string? CtaCble { get; set; }
    }
}
