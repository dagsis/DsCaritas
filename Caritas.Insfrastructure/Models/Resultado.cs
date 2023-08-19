using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caritas.Insfrastructure.Models
{
    public class Resultado
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public string Email { get; set; }
        public string Result { get; set; }
    }
}
