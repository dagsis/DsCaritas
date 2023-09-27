using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caritas.Insfrastructure.Models
{
    public class Aviso
    {
        public int Id { get; set; }
        public int Cliente { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string CPagoElectronico { get; set; }
        public int NroControl { get; set; }
        public string Tipo { get; set; }
        public int Piso { get; set; }
        public int Nicho { get; set; }
        public DateTime FecDesde { get; set; }
        public DateTime FecHasta { get; set; }
        public decimal Importe { get; set; }
        public string Domicilio { get; set; }
        public string Valor_Vencido { get; set; }
        public string Valor_a_Vencer { get; set; }
        public string SiguienteValor_Vencido { get; set; }
        public string SiguienteValor_a_Vencer { get; set; }
        public string PisoChar { get; set; }
        public string Email3 { get; set; }
    }
}
