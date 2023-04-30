using Caritas.Insfrastructure.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caritas.Insfrastructure.Model
{
    public class Ubicacion : BaseDomainModel
    {
        public int Nicho { get; set; }
        public int Piso { get; set; }
        public int Fila { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? TipoUbicacion { get; set; }

        [Column(TypeName = "varchar(1)")]
        public string? EstadoU { get; set; }
    }
}
