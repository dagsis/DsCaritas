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
        public string Periodo { get; set; }
        public string Cuota { get; set; }
        public string Anio { get; set; }
        public DateTime Vencimiento { get; set; }
    }
}
