using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caritas.Insfrastructure.Models
{
    public class Calendario
    {
        public int Id { get; set; }
        public DateTime Vencimiento { get; set; }
        public DateTime VencimientoProc  { get; set; }
        public DateTime FechaAPartir { get; set; }
        public string Observacion { get; set; }
    }
}
