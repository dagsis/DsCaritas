
using Caritas.Insfrastructure.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caritas.Insfrastructure.Model
{
    public class EstadoC : BaseDomainModel
    {
        [Column(TypeName = "varchar(1)")]
        public string? Codigo { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string? Nombre { get; set; }
    }
}
